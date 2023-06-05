using Azure.Data.Tables;
using POCEventSourcing.Core;

namespace POCEventSourcing.ReplicationJob
{
    internal static class Extensions
    {
        public static EntityChangesTrackerEventEntry ToEntityEventEntry(this TableEntity tableEntity)
        {
            var typeOfDateTime = typeof(DateTime);
            var typeOfDateTimeNullable = typeof(DateTime?);
            var typeOfDateTimeOffSet = typeof(DateTimeOffset);
            var typeOfDateTimeOffsetNullable = typeof(DateTimeOffset?);

            var properties = typeof(EntityChangesTrackerEventEntry).GetProperties();
            var instance = new EntityChangesTrackerEventEntry();

            foreach(var property in properties)
            {
                object? value = null;
                var sucess = tableEntity.TryGetValue(property.Name, out value);

                if(!sucess)
                {
                    continue;
                }

                if (property.PropertyType.IsEnum)
                {
                    var enumValue = Enum.Parse(property.PropertyType, value?.ToString(), true);
                    property.SetValue(instance, enumValue);
                }
                else if ( 
                    (property.PropertyType.Equals(typeOfDateTime) || property.PropertyType.Equals(typeOfDateTimeNullable)) ||
                    (property.PropertyType.Equals(typeOfDateTimeOffSet) || property.PropertyType.Equals(typeOfDateTimeOffsetNullable))
                )
                {
                    var strValue = value?.ToString();

                    if(string.IsNullOrEmpty(strValue) || string.IsNullOrWhiteSpace(strValue))
                    {
                        if(property.PropertyType.IsGenericType && property.PropertyType.UnderlyingSystemType.Equals(typeOfDateTime))
                        {
                            property.SetValue(instance, null);
                            continue;
                        }
                    }

                    object dateValue = null;

                    if (property.PropertyType.Equals(typeOfDateTime) || property.PropertyType.Equals(typeOfDateTimeNullable))
                    {
                        dateValue = DateTime.Parse(strValue);
                    }
                    else if (property.PropertyType.Equals(typeOfDateTimeOffSet) || property.PropertyType.Equals(typeOfDateTimeOffsetNullable))
                    {
                        dateValue = DateTimeOffset.Parse(strValue);
                    }

                    property.SetValue(instance, dateValue);
                }
                else
                {
                    property.SetValue(instance, value);
                }
            }

            return instance;
        }
    }
}
