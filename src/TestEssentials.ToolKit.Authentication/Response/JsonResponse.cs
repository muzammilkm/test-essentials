using Newtonsoft.Json;

namespace TestEssentials.ToolKit.Authentication.Response
{
    public class JsonResponse<T> : Response
    {
        public JsonResponse(string responseContent)
            : base(responseContent)
        {
            Object = JsonConvert.DeserializeObject<T>(responseContent);
        }

        public T Object { get; }

    }
}
