using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Topshelf;
using WireMock.Handlers;
using WireMock.Logging;
using WireMock.Net.StandAlone;
using WireMock.Server;
using WireMock.Settings;

namespace TestEssentials.ToolKit.MockServer
{
    public class WireMockService : ServiceControl
    {
        private readonly WireMockConsoleLogger _logger;
        private readonly IFileSystemHandler _fileHandler;
        private readonly bool _useSsl;
        private readonly int? _port;

        private WireMockServer _server;
        private readonly string _assemblyFile;
        private readonly string _typeName;

        #region Ctor
        public WireMockService(string map, int port, bool useSsl, string assemblyFile, string typeName)
        {
            _port = port;
            _useSsl = useSsl;
            _fileHandler = new LocalFileSystemHandler(map);
            _assemblyFile = assemblyFile;
            _typeName = typeName;
            _logger = new WireMockConsoleLogger();
        }
        #endregion

        public bool Start(HostControl hostControl)
        {

            _logger.Debug("Starting...");

            var handleBarTransformers = new List<IHelperDescriptor<BlockHelperOptions>>();

            try
            {
                var currentAssembly = Assembly.LoadFrom(_assemblyFile);
                var item = currentAssembly.GetType(_typeName);
                handleBarTransformers.Add((IHelperDescriptor<BlockHelperOptions>)Activator.CreateInstance(item));
            }
            catch (Exception ex)
            {
                // ignored
            }


            var settings = new WireMockServerSettings()
            {
                UseSSL = _useSsl,
                Port = _port,
                ReadStaticMappings = true,
                WatchStaticMappings = true,
                WatchStaticMappingsInSubdirectories = true,
                AllowPartialMapping = true,
                StartAdminInterface = true,
                Logger = _logger,
                FileSystemHandler = _fileHandler,
                HandlebarsRegistrationCallback = (a, b) =>
                {
                    foreach (var handleBarTransformer in handleBarTransformers)
                    {
                        a.RegisterHelper(handleBarTransformer);
                    }
                }
            };
            _server = StandAloneApp.Start(settings);
            _logger.Debug("Started");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.Debug("Stopping...");
            _server.Stop();
            _logger.Debug("Stopped");
            return true;
        }

    }
}
