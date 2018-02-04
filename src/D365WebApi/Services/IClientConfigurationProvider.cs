using D365WebApi.Core;

namespace D365WebApi.Services
{
    public interface IClientConfigurationProvider
    {
        ClientConfiguration ReadFromDisk(string configName);
    }
}