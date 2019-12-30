using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestEssentials.ToolKit.Authentication.Payload
{
    public class PayloadReader
    {
        public static async Task<JsonPayload<T>> ReadJsonFileAsync<T>(string fileName)
        {
            var jsonContent = await Task.Run(() => File.ReadAllText($"{fileName}.json"));
            return new JsonPayload<T>(jsonContent);
        }

        public static async Task<Payload> ReadJsonFileAsync(string fileName)
        {
            var jsonContent = await Task.Run(() => File.ReadAllText($"{fileName}.json"));
            return new Payload(jsonContent);
        }
    }
}
