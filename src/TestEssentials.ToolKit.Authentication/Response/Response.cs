namespace TestEssentials.ToolKit.Authentication.Response
{
    public class Response
    {
        public Response(string responseContent)
        {
            Content = responseContent;
        }

        public string Content { get; }
    }
}
