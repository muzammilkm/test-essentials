using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TestEssentials.ToolKit.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient RemoveBearerToken(this HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = null;
            return client;
        }

        public static HttpClient SetTestBearerToken(this HttpClient client, IDictionary<string, object> bearerToken)
        {
            var token = JsonConvert.SerializeObject(bearerToken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
