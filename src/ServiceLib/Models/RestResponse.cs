using System.Net;
using System.Net.Http;

namespace ServiceLib.Models
{
    public class RestResponse
    {
        public RestResponse() {}
        public RestResponse(HttpRequestException ex) { HttpException = ex; }

        public HttpStatusCode? StatusCode { get; set; }
        public string? Body { get; set; }
        public HttpRequestException? HttpException { get; set; }
    }
}