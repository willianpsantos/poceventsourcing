using Microsoft.Extensions.Configuration;
using POCEventSourcing.Options;
using System.Reflection;

namespace POCEventSourcing.IoC
{
    internal class TenantOptionsBuilder
    {
        private readonly IConfiguration _configuration;
        
        private string _environmentName;

        public TenantOptionsBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TenantOptionsBuilder Environment(string name)
        {
            _environmentName = name;
            return this;
        }

        public TenantOptions Build()
        {
            var envName = _environmentName;

            if(string.IsNullOrEmpty(envName) || string.IsNullOrWhiteSpace(envName))
            {
                envName = _configuration.GetCurrentTenantEnvironment();
            }

            var env = new TenantOptions();

            var configBuilder = new ConfigurationBuilder();

            var tenantConfig =
                configBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables(envName)
                    .AddUserSecrets(Assembly.GetEntryAssembly(), true)
                    .AddJsonFile($"{envName}.tenant.json")
                    .Build();

            tenantConfig.Bind(env);

            return env;
        }

        public string GetWritableDatabaseConnectionString(TenantOptions tenantOptions)
        {
            var conStr = tenantOptions.WritableDatabaseOptions.ConnectionString;

            return conStr;
        }
    }
}
