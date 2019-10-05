using API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using TestEssentials.ToolKit.Database;
using TestEssentials.ToolKit.Database.SqlServer;

namespace UIInit
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDatabase();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static void CreateDatabase()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var database = new DacpacTestDatabase(config.GetValue<string>("ConnectionStrings:DBConnection"))
                .ConfigureOptions(option =>
                {
                    var dacpacPath = Path.GetFullPath(
                                        @"..\..\..\..\..\WeatherApp\DB\bin\Debug\db.dacpac");
                    option.DacpacPath = dacpacPath;
#if DEBUG
                    option.AlwayCreate = false;
                    option.AlwayDrop = false;
#else
                    option.AlwayCreate = false;
                    option.AlwayDrop = true;
#endif
                })
                .Build()
                .RunScriptFile(@"Scripts\clean-all-stations.sql")
                .RunScriptFile(@"Scripts\add-ap-stations.sql")
                .RunScriptFile(@"Scripts\add-ka-stations.sql")
                .RunScriptFile(@"Scripts\add-ks-stations.sql")
                .RunScriptFile(@"Scripts\add-tn-stations.sql")
                .RunScriptFile(@"Scripts\add-ts-stations.sql");

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(o =>
                {
                    o.Listen(IPAddress.Loopback, 4200);
                })
                .UseStartup<Startup>();
    }
}
