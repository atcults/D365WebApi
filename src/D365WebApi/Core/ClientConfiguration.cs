namespace D365WebApi.Core
{
    public class ClientConfiguration
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Region { get; set; }

        public string InstanceName { get; set; }

        public string TenantId { get; set; }

        public string AuthorityEndPoint => $"https://login.microsoftonline.com/{TenantId}/oauth2/token";

        public string Resource => $"https://{InstanceName}.{Region}.dynamics.com";

        public string ODataEndPoint => $"{Resource}/api/data/v9.0/";

    }
}
