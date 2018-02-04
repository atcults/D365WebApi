using System;
using System.IO;
using D365WebApi.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace D365WebApi.Services.Impl
{
    public class ClientConfigurationProvider : IClientConfigurationProvider
    {
        private readonly ILogger<ClientConfigurationProvider> _logger;

        public ClientConfigurationProvider(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ClientConfigurationProvider>();
        }

        public ClientConfiguration ReadFromDisk(string configName)
        {
            var appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "D365Client");

            if (!Directory.Exists(appDataFolder))
            {
                _logger.LogCritical($"Configuration foler does not exist. Application will create foler {appDataFolder} and raise error");
                Directory.CreateDirectory(appDataFolder);
            }

            var filePath = Path.Combine(appDataFolder, $"{configName}.json");

            var serializer = new JsonSerializer();

            if (!File.Exists(filePath))
            {
                _logger.LogCritical($"Configuration foler does not exist. Application will create foler {appDataFolder} and raise error");

                using (var file = File.CreateText(filePath))
                {
                    serializer.Serialize(file, new ClientConfiguration
                    {
                        ClientId = "--YOUR CLIENT ID--",
                        ClientSecret = "--YOUR CLIENT SECRET--",
                        Region = "--YOUR REGION NAME--",
                        TenantId = "--YOUR TENANT ID--",
                        InstanceName = "--YOUR INSTANCE NAME--"
                    });
                }

                throw new ApplicationException($"Configuration file {filePath} was not exist. We have created dummy file. Please update with real values");
            }

            ClientConfiguration config;

            // deserialize JSON directly from a file
            using (var file = File.OpenText(filePath))
            {
                config = (ClientConfiguration) serializer.Deserialize(file, typeof(ClientConfiguration));
            }

            if (config.Region.Equals("--YOUR REGION NAME--"))
            {
                throw new ApplicationException("Configuration file is invalid. Please update with real values");
            }

            return config;
        }        
    }
}