using Azure.Data.Tables;
using Newtonsoft.Json;
using POCEventSourcing.Core;
using POCEventSourcing.Enums;
using System.Reflection;

namespace POCEventSourcing.Trackers
{
    public abstract class EntityChangesTrackingStorage
    {
        protected readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        protected virtual IDictionary<string, object?> GetModifiedMembers<TEntity>(TEntity original, TEntity updated) where TEntity: Entity
        {
            Type typeOfEntity = typeof(TEntity);
            Type typOfIgnoreAttribute = typeof(TrackerIgnoreAttribute);
            PropertyInfo[] properties = typeOfEntity.GetProperties();
            Dictionary<string, object?> membersModified = new Dictionary<string, object?>(properties.Count());

            foreach (var property in properties)
            {
                var ignoreAttributes = property.GetCustomAttributes(typOfIgnoreAttribute, true);

                if (ignoreAttributes != null && ignoreAttributes.Count() > 0)
                {
                    continue;
                }

                var originalValue = property.GetValue(original);
                var updatedValue = property.GetValue(updated);

                if ((originalValue == null && updatedValue != null) || (originalValue != null && updatedValue == null))
                {
                    membersModified.Add(property.Name, updatedValue);
                    continue;
                }

                if (originalValue.Equals(updatedValue))
                {
                    continue;
                }

                membersModified.Add(property.Name, updatedValue);
            }

            return membersModified;
        }

        protected virtual EntityChangesTrackerEventEntry GetEntityTrackerEventEntry<TEntity>
        (
            EntityEventState state, 
            TEntity original, 
            TEntity? updated = null
        ) 
        where TEntity : Entity
        {
            if (original is null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            DateTime currentDate = DateTime.UtcNow;
            Type typeOfEntity = original.GetType();
            Entity baseEntity = (Entity)original;

            EntityChangesTrackerEventEntry? eventEntry = new EntityChangesTrackerEventEntry
            {
                RowKey = baseEntity.GetRowKey(currentDate),
                EntityName = typeOfEntity.Name,
                EntityId = baseEntity.Id,
                State = state,
                EventDate = currentDate
            };

            switch(state)
            {
                case EntityEventState.Deleted:
                case EntityEventState.Added:
                    eventEntry.Original = JsonConvert.SerializeObject(original, _serializerSettings);
                    eventEntry.Updated = null;
                    break;
                case EntityEventState.Modified:
                    //IDictionary<string, object?> membersModified = GetModifiedMembers(original, updated);
                    eventEntry.Original = JsonConvert.SerializeObject(original, _serializerSettings);
                    eventEntry.Updated = JsonConvert.SerializeObject(updated, _serializerSettings);
                    break;
            }           
            
            return eventEntry;
        }

        protected virtual EntityChangesTrackerEvent GetEntityTrackerEvent<TEntity>
        (
            string partitionKey,  
            IEnumerable<EntityEventEntry<TEntity>> entitiesEntry
        )
        where TEntity : Entity
        {
            var entries = 
                entitiesEntry
                    .Select(e => GetEntityTrackerEventEntry(e.State, e.Original, e.Updated))
                    .ToArray();

            var eventTracker = new EntityChangesTrackerEvent
            {
                PartitionKey = partitionKey,
                Entries = entries
            };

            return eventTracker;
        }

        protected virtual IEnumerable<TableEntity> GetTableEntities(EntityChangesTrackerEvent eventTracker)
        {
            HashSet<TableEntity> tableEntities = new HashSet<TableEntity>();

            foreach (var item in eventTracker.Entries)
            {
                var tableEntity = item.ToTableEntity(eventTracker.PartitionKey, item.RowKey);
                tableEntities.Add(tableEntity);
            }

            return tableEntities.ToArray();
        }
    }
}
