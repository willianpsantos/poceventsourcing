using Newtonsoft.Json;

namespace POCEventSourcing.ReplicationJob
{
    internal static class EntityNameTypeMapper
    {
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>();

        internal static void LoadMappings()
        {
            var appPath = AppContext.BaseDirectory;

            using (StreamReader reader = new StreamReader($"{appPath}/entity.name.mappings.json"))
            {
                var json = reader.ReadToEnd();
                _mappings = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
            }
        }

        internal static Type? GetEntityType(string name)
        {
            if(!_mappings.ContainsKey(name))
            {
                return null;
            }

            var className = _mappings[name];
            var type = Type.GetType(className);

            return type;
        }
    }
}
