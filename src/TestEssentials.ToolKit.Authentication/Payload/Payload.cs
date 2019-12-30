using System.Net.Http;
using System.Text;

namespace TestEssentials.ToolKit.Authentication.Payload
{
    public class Payload
    {
        public Payload(string jsonContent)
        {
            Json = jsonContent;
            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        }

        public string Json { get; }

        public StringContent Content { get; }
    }
}
