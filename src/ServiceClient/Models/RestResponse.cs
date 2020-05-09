using System.Net;
using System.Net.Http;

namespace ServiceClient.Models
{
    public class RestResponse
    {
        public HttpStatusCode? StatusCode { get; set; }
        public string Body { get; set; }
        public HttpRequestException HttpException { get; set; }
    }
}