using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace POCEventSourcing.IoC
{
    public class ManualDependencyInjection
    {
        private IServiceCollection? _serviceCollection;
        private ServiceProvider? _serviceProvider;
        private IConfiguration? _configuration;

        private bool _buildConfiguration = true;
        private string? _appSettingPath = "appsettings.json";

        internal ManualDependencyInjection()
        {
            
        }

        public ManualDependencyInjection BuildConfiguration(bool value)
        {
            _buildConfiguration = value;

            return this;
        }

        public ManualDependencyInjection AppSettingsPath(string? value)
        {
            _appSettingPath = value;

            return this;
        }

        internal void Init()
        {
            var serviceCollection = new ServiceCollection();

            if (_buildConfiguration)
            {
                var configBuilder = new ConfigurationBuilder();
                var config = configBuilder.AddJsonFile(_appSettingPath).Build();

                _configuration = config;
            }

            serviceCollection.AddAllDependencies(_configuration);

            _serviceCollection = serviceCollection;
            _serviceProvider = _serviceCollection.BuildServiceProvider();            
        }

        public IConfiguration? GetConfiguration()
        {
            return _configuration;
        }

        public T GetService<T>() where T : notnull
        {
            var service = _serviceProvider.GetRequiredService<T>();

            return service;
        }
    }
}
