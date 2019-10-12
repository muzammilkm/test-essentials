using System;
using HandlebarsDotNet;
using WireMock.Handlers;
using WireMock.Server;
using WireMock.Settings;

namespace TestEssentials.ToolKit.WireMock
{
    [Obsolete]
    public class WireMockServer
    {
        private FluentMockServer _server;        
        private readonly IFluentMockServerSettings _settings;

        #region ctor
        public WireMockServer(string mappingPath, int? port = null, Action<IHandlebars, IFileSystemHandler> handlebarsRegistrationCallback = null)
        {
            _settings = new FluentMockServerSettings()
            {
                ReadStaticMappings = true,
                WatchStaticMappings = true,
                AllowPartialMapping = true,
                StartAdminInterface = true,
                Port = port ?? 36987,
                FileSystemHandler = new LocalFileSystemHandler(mappingPath),
                HandlebarsRegistrationCallback = handlebarsRegistrationCallback
            };
        }
        #endregion

        /// <summary>
        /// Starts mock server.
        /// </summary>
        public void Start()
        {
            if (_server != null && _server.IsStarted)
                Stop();
            _server = FluentMockServer.Start(_settings);
        }

        /// <summary>
        /// Stops mock server.
        /// </summary>
        public void Stop()
        {
            if (_server != null && _server.IsStarted)
                _server.Stop();
        }
    }
}
