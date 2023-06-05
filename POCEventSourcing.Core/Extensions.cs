using Azure.Data.Tables;

namespace POCEventSourcing.Core
{
    public static class Extensions
    {
        public static string ToTimeStampString(this DateTime date)
        {
            var year = date.Year.ToString();
            var month = date.Month.ToString().PadLeft(2, '0');
            var day = date.Month.ToString().PadLeft(2, '0');
            var hour = date.Hour.ToString().PadLeft(2, '0');
            var minute = date.Minute.ToString().PadLeft(2, '0');
            var second = date.Second.ToString().PadLeft(2, '0');
            var millisend = date.Millisecond.ToString().PadLeft(2, '0');

            return $"{year}{month}{day}{hour}{minute}{second}{millisend}";
        }

        public static TableEntity ToTableEntity(this object obj, string partitionKey, string rowkey)
        {
            var properties = obj.GetType().GetProperties();
            var typeOfIgnoreAttribute = typeof(TrackerIgnoreAttribute);
            var tableEntity = new TableEntity(partitionKey, rowkey);

            foreach(var property in properties)
            {
                var isIgnorable = property.GetCustomAttributes(typeOfIgnoreAttribute, true);

                if(isIgnorable?.Length > 0)
                {
                    continue;
                }

                var value = property.GetValue(obj, null);

                if (property.PropertyType.IsEnum)
                {
                    var name = Enum.GetName(property.PropertyType, value);
                    value = name;
                }

                tableEntity.Add(property.Name, value);
            }

            return tableEntity;
        }
    }
}
