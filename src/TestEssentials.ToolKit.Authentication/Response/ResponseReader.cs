using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestEssentials.ToolKit.Authentication.Response
{
    public class ResponseReader
    {
        public static async Task<JsonResponse<T>> ReadAync<T>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var actualQuestion = JsonConvert.DeserializeObject<T>(responseContent);

            return new JsonResponse<T>(responseContent);
        }

        public static async Task<Response> ReadAync(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return new Response(responseContent);
        }
    }
}
