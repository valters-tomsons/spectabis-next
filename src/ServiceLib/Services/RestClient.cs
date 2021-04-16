using System;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceLib.Interfaces;
using ServiceLib.Models;

namespace ServiceLib.Services
{
    public class RestClient : IRestClient
    {
        private HttpClient HttpClient { get; set; }
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
            if (HttpClient == null)
            {
                throw new Exception("HttpClient not initialized!");
            }

            var requestMessage = CreateFunctionMessage(HttpMethod.Get, source, query);
            var response = await HttpClient.SendAsync(requestMessage).ConfigureAwait(false);

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        public HttpClient GetClient()
        {
            if(HttpClient == null)
            {
                throw new Exception("HttpClient not initialized!");
            }

            return HttpClient;
        }

        private void InitializeHttpClient()
        {
            if (HttpClient == null)
            {
                lock(ClientSemaphore)
                {
                    HttpClient = new HttpClient();
                    HttpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-functions-key", _functionKey);
                    HttpClient.BaseAddress = _functionBaseUrl;
                }
            }
        }

        private async Task<RestResponse> CommonFunctionRequest(Uri endpoint, HttpMethod method, string query)
        {
            if (HttpClient == null)
            {
                throw new Exception("HttpClient not initialized!");
            }

            var requestMessage = CreateFunctionMessage(method, endpoint, query);

            try
            {
                var response = await HttpClient.SendAsync(requestMessage).ConfigureAwait(false);
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