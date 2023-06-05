using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POCEventSourcing.ReplicationJob.DB;
using POCEventSourcing.ReplicationJob.Interfaces;

namespace POCEventSourcing.ReplicationJob
{
    internal static class EntityProcessorTypeMapper
    {
        private static readonly Type _defaultProcessor = typeof(Processors.EntityEventTrackerProcessor);
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>();
        

        internal static void LoadMappings()
        {
            var appPath = AppContext.BaseDirectory;

            using (StreamReader reader = new StreamReader($"{appPath}/entity.processors.mappings.json"))
            {
                var json = reader.ReadToEnd();
                _mappings = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
            }
        }

        internal static IEntityEventTrackerProcessor GetProcessor(string name)
        {
            Type? processorType = null;

            if (!_mappings.ContainsKey(name))
            {
                processorType = _defaultProcessor;
            }
            else
            {
                var className = _mappings[name];
                processorType = Type.GetType(className);
            }

            var context = DependencyResolver.GetService<POCReplicationDbContext>();
            var constructor = processorType.GetConstructor(new Type[] { typeof(POCReplicationDbContext) });
            var instance = constructor.Invoke(new[] { context });

            return (IEntityEventTrackerProcessor)instance;
        }
    }
}
