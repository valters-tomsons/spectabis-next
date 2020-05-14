using System;
using System.Threading.Tasks;
using ServiceClient.Models;

namespace ServiceClient.Interfaces
{
    public interface IRestClient
    {
        void SetSession(string functionKey, Uri functionBaseUrl);
        Task<RestResponse> GetFunctionRequest(Uri endpoint, string query = null);
        Task<byte[]> GetBytesAsync(Uri source, string query = null);
    }
}