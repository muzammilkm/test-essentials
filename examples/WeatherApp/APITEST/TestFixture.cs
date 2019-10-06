using API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using TestEssentials.ToolKit.Database;
using TestEssentials.ToolKit.Database.Dacpac;

namespace TEST
{
    public class TestFixture : IDisposable
    {
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

            var builder = new WebHostBuilder()
                .UseConfiguration(config)
                .UseStartup<Startup>();

            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://local.sandbox");
        }

        #endregion

        public HttpClient Client { get; }


        public void Dispose()
        {
            Client.Dispose();
            _database.Dispose();
            _server.Dispose();
        }
    }
}
