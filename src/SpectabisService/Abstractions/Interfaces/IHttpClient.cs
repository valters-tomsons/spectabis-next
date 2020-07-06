using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpectabisService.Abstractions.Interfaces
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(Uri requestUri);
    }
}