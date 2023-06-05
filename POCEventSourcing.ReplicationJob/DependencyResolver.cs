namespace POCEventSourcing.ReplicationJob
{
    internal static class DependencyResolver
    {
        private static IServiceProvider _serviceProvider;
        
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>() => _serviceProvider.GetService<T>();
    }
}
