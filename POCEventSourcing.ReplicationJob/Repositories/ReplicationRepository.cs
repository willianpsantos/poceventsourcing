using POCEventSourcing.Core;
using POCEventSourcing.ReplicationJob.Entities;

namespace POCEventSourcing.ReplicationJob.Repositories
{
    internal class ReplicationRepository
    {
        protected virtual void AssignUpdatedProps<TEntity>(ref TEntity original, ref TEntity updated) where TEntity : Entity
        {
            var typeOfReplicationEntity = typeof(ReplicationEntity);
            var typeOfEntity = typeof(Entity);
            var properties = typeof(TEntity).GetProperties();

            foreach (var prop in properties)
            {
                var propType = prop.PropertyType;

                if (propType.BaseType != null && (propType.BaseType.Equals(typeOfReplicationEntity) || propType.BaseType.Equals(typeOfEntity)))
                {
                    continue;
                }

                var originalValue = prop.GetValue(original);
                var updatedValue = prop.GetValue(updated);

                if (updatedValue == originalValue)
                {
                    continue;
                }

                prop.SetValue(original, updatedValue);
            }
        }
    }
}
