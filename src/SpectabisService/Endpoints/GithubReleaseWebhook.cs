using System.Security.Cryptography;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;

namespace SpectabisService.Endpoints
{
    public class GithubReleaseWebhook
    {
        private static string GithubWebhooksSecret;

        public GithubReleaseWebhook(IConfigurationRoot config)
        {
            if(string.IsNullOrEmpty(GithubWebhooksSecret))
            {
                GithubWebhooksSecret = config.GetValue<string>("GithubWebhooksSecret");
            }
        }

        [FunctionName("GithubReleaseEvent")]
        public async Task<IActionResult> ReleaseEvent([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            var signed = req.Headers.TryGetValue("X-Hub-Signature", out var secrets);
            var reqSignature = secrets.FirstOrDefault();

            if(!signed || string.IsNullOrEmpty(reqSignature))
            {
                return new UnauthorizedResult();
            }

            var body = await req.ReadAsStringAsync().ConfigureAwait(false);
            var signature = GetSHA1Signature(body);

            if(signature != reqSignature)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(new {signature, reqSignature});
        }

        private string GetSHA1Signature(string body)
        {
            var keyBytes = Encoding.UTF8.GetBytes(GithubWebhooksSecret);
            var textBytes = Encoding.UTF8.GetBytes(body);

            byte[] hashBytes;

            using(var hash = new HMACSHA1(keyBytes))
            {
                hashBytes = hash.ComputeHash(textBytes);
            }

            var hashHex = new StringBuilder(hashBytes.Length);
            for(int i = 0; i < hashBytes.Length; i++)
            {
                hashHex.Append(hashBytes[i].ToString("x2"));
            }

            return $"sha1={hashHex}";
        }
    }
}