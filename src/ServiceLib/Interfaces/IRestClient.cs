using System;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceLib.Models;

namespace ServiceLib.Interfaces
{
    public interface IRestClient
    {
        void SetSession(string functionKey, Uri functionBaseUrl);
        Task<RestResponse> GetFunctionRequest(Uri endpoint, string? query = null);
        Task<byte[]?> GetBytesAsync(Uri source, string? query = null);
        HttpClient GetClient();
    }
}