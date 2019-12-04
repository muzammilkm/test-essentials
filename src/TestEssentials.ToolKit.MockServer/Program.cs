using System;
using System.Linq;
using Topshelf;

namespace TestEssentials.ToolKit.MockServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Any(x => x.Contains("--help")))
            {
                DisplayHelp();
                Environment.ExitCode = 0;
                return;
            }

            var map = "";
            var useSsl = false;
            var port = 36987;
            var assemblyFile = "";
            var typeName = "";


            var topshelfExitCode = HostFactory.Run(x =>
            {
                x.AddCommandLineDefinition("map", v => map = v);
                x.AddCommandLineDefinition("port", v => port = Convert.ToInt32(v));
                x.AddCommandLineSwitch("ssl", v => useSsl = v);
                x.AddCommandLineDefinition("assembly", v => assemblyFile = v);
                x.AddCommandLineDefinition("types", v => typeName = v);

                x.Service<WireMockService>(sc =>
                {
                    sc.ConstructUsing(() => new WireMockService(map, port, useSsl, assemblyFile, typeName));
                    sc.WhenStarted((s, hc) => s.Start(hc));
                    sc.WhenStopped((s, hc) => s.Stop(hc));
                });
                x.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(5);
                });
                x.RunAsLocalSystem();
                x.SetDescription("Mock Server");
                x.SetDisplayName("Mock Server");
                x.SetServiceName("MockServer");
            });

            var exitCode = (int) Convert.ChangeType(topshelfExitCode, topshelfExitCode.GetTypeCode());
            Environment.ExitCode = exitCode;
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Mock Server");
            Console.WriteLine("");
            Console.WriteLine("Useage: ");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("\t--help\t\tShow help");
            Console.WriteLine("\t--map\t\tLocation to mapping files.");
            Console.WriteLine("\t--ssl\t\tEnable SSL. [default: false]");
            Console.WriteLine("\t--port\t\tPort to listen on [default:36987].");
            Console.WriteLine("\t--assembly\t\tLoad custom handlebar assembly [default:''].");
            Console.WriteLine("\t--type\t\tComplete Namespace of custom handlebar in assembly [default:''].");
            Console.WriteLine("");
        }
    }
}
