using API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TestEssentials.ToolKit.Database;
using TestEssentials.ToolKit.Database.SqlServer;
using TestEssentials.ToolKit.Database.SqlServer.Options;

namespace TEST
{
    public class TestFixture : IDisposable
    {

        private readonly CancellationTokenSource cts;

        private readonly TestServer _server;
        private readonly ITestDatabase _database;

        #region Ctor
        public TestFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _database = new DacpacTestDatabase(config.GetValue<string>("ConnectionStrings:DBConnection"))
                .ConfigureOptions(option =>
                {
                    var dacpacPath = Path.GetFullPath(
                                        @"..\..\..\..\..\WebApp\DB\bin\Debug\db.dacpac");
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

            //var builder = new WebHostBuilder()
            //    .UseConfiguration(config)
            //    .UseStartup<Startup>();

            var builder = WebHost.CreateDefaultBuilder().UseStartup<Startup>();
            cts = new CancellationTokenSource();

            builder
                .UseKestrel(o =>
                {
                    o.Listen(IPAddress.Loopback, 4200);
                })
                .Build()
                .RunAsync(cts.Token);

            //_serverTask.Start();
            //_server = new TestServer(builder);

            Client = new HttpClient(); //_server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:4200");
        }

        private readonly Task _serverTask;
        #endregion

        public HttpClient Client { get; }


        public void Dispose()
        {
            Client.Dispose();
            _database.Dispose();
            //_server.Dispose();
            cts.Cancel();
        }
    }
}
