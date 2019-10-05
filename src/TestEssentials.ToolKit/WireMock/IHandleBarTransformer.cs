using System.IO;

namespace TestEssentials.ToolKit.WireMock
{
    public interface IHandleBarTransformer
    {
        string Name { get; }

        void Render(TextWriter textWriter, dynamic context, object[] arguments);
    }
}
