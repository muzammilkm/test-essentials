using Newtonsoft.Json;

namespace TestEssentials.ToolKit.Authentication.Payload
{
    public class JsonPayload<T> : Payload
    {
        public JsonPayload(string jsonContent)
            : base(jsonContent)
        {
            Object = JsonConvert.DeserializeObject<T>(jsonContent); ;
        }

        public T Object { get; }
    }
}
