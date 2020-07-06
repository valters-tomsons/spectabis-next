using System;
using System.Net.Http;
using System.Threading.Tasks;
using SpectabisService.Abstractions.Interfaces;

namespace SpectabisService.Abstractions
{
    public class HttpClientFacade : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientFacade()
        {
            if(_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
        }
    }
}