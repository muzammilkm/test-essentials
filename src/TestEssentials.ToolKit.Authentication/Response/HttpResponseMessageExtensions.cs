using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestEssentials.ToolKit.Authentication.Response
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<JsonResponse<T>> ReadAync<T>(this HttpResponseMessage response)
        {
            return await ResponseReader.ReadAync<T>(response);
        }

        public static async Task<Response> ReadAync(this HttpResponseMessage response)
        {
            return await ResponseReader.ReadAync(response);
        }
    }
}
