using System;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceClient.Interfaces;
using ServiceClient.Models;

namespace ServiceClient.Services
{
    public class RestClient : IRestClient
    {
        private HttpClient _httpClient { get; set; }
        private static readonly object ClientSemaphore = new object();
        private const string UserAgent = "SpectabisLib/(.NET Core)";

        private string _functionKey;
        private Uri _functionBaseUrl;

        public RestClient() { }

        public void SetSession(string functionKey, Uri functionBaseUrl)
        {
            _functionKey = functionKey;
            _functionBaseUrl = functionBaseUrl;
            InitializeHttpClient();
        }

        public async Task<RestResponse> GetFunctionRequest(Uri endpoint, string query = null)
        {
            return await CommonFunctionRequest(endpoint, HttpMethod.Get, query).ConfigureAwait(false);
        }

        public async Task<byte[]> GetBytesAsync(Uri source, string query = null)
        {
            if (_httpClient == null)
            {
                throw new Exception("HttpClient not initialized!");
            }

            var requestMessage = CreateFunctionMessage(HttpMethod.Get, source, query);
            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        public HttpClient GetClient()
        {
            if(_httpClient == null)
            {
                throw new Exception("HttpClient not initialized!");
            }

            return _httpClient;
        }

        private void InitializeHttpClient()
        {
            if (_httpClient == null)
            {
                lock(ClientSemaphore)
                {
                    _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-functions-key", _functionKey);
                    _httpClient.BaseAddress = _functionBaseUrl;
                }
            }
        }

        private async Task<RestResponse> CommonFunctionRequest(Uri endpoint, HttpMethod method, string query)
        {
            if (_httpClient == null)
            {
                throw new Exception("HttpClient not initialized!");
            }

            var requestMessage = CreateFunctionMessage(method, endpoint, query);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return new RestResponse()
                {
                    StatusCode = response.StatusCode,
                    Body = responseBody,
                    HttpException = null
                };
            }
            catch (HttpRequestException e)
            {
                return new RestResponse(e);
            }
        }

        private HttpRequestMessage CreateFunctionMessage(HttpMethod httpMethod, Uri requestUri, string query)
        {
            var uriBuilder = new UriBuilder(_functionBaseUrl)
            {
                Query = query
            };

            uriBuilder.Path += $"{requestUri}";

            return new HttpRequestMessage(httpMethod, uriBuilder.Uri);
        }
    }
}