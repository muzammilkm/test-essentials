using System;
using HandlebarsDotNet;
using WireMock.Handlers;
using WireMock.Server;
using WireMock.Settings;

namespace TestEssentials.ToolKit.WireMock
{
#pragma warning disable 0618
    public class HttpMockServer
    {
        private WireMockServer _server;
        private readonly WireMockServerSettings _settings;

        #region ctor
        public HttpMockServer(string mappingPath, int? port = null, Action<IHandlebars, IFileSystemHandler> handlebarsRegistrationCallback = null)
        {
            _settings = new WireMockServerSettings()
            {
                ReadStaticMappings = true,
                WatchStaticMappings = true,
                WatchStaticMappingsInSubdirectories = true,
                AllowPartialMapping = true,
                StartAdminInterface = true,
                Port = port ?? 36987,
                FileSystemHandler = new LocalFileSystemHandler(mappingPath),
                HandlebarsRegistrationCallback = handlebarsRegistrationCallback
            };
        }

        public HttpMockServer(WireMockServerSettings wireMockServerSettings)
        {
            _settings = wireMockServerSettings;
        }
        #endregion

        /// <summary>
        /// Starts mock server.
        /// </summary>
        public void Start()
        {
            if (_server != null && _server.IsStarted)
                Stop();
            _server = WireMockServer.Start(_settings);
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
