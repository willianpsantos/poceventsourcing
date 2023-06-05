namespace POCEventSourcing.IoC
{
    public class ManualDependencyInjectionBuilder
    {        
        private bool _buildConfiguration = true;
        private string? _appSettingPath = "appsettings.json";

        public ManualDependencyInjectionBuilder()
        {
            
        }

        public ManualDependencyInjectionBuilder BuildConfiguration(bool value)
        {
            _buildConfiguration = value;

            return this;
        }

        public ManualDependencyInjectionBuilder AppSettingsPath(string value)
        {
            _appSettingPath = value;

            return this;
        }

        public ManualDependencyInjection Build()
        {
            var di = new ManualDependencyInjection();

            di.AppSettingsPath(_appSettingPath)
              .BuildConfiguration(_buildConfiguration)
              .Init();

            return di;
        }
    }
}
