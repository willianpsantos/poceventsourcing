using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POCEventSourcing.Core;
using POCEventSourcing.Interfaces.DB;
using System.Linq;

namespace POCEventSourcing.DB.Managers
{
    public  class WritableDbEntityStateManager : IWritableDbEntityStateManager
    {
        private readonly DbContext _context;
        private readonly HashSet<object> _entityEventEntries;

        public WritableDbEntityStateManager(DbContext context)
        {
            _context = context;
            _entityEventEntries = new HashSet<object>();
        }


        private EntityEventEntry<TEntity> AssignUpdatedProps<TEntity>(ref TEntity original, ref TEntity updated) where TEntity : Entity
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };

            var clonedOriginal = JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(original, serializerSettings), serializerSettings);

            var entry = new EntityEventEntry<TEntity>()
            {
                Original = clonedOriginal,
                Updated = updated,
                State = Enums.EntityEventState.Modified,
                EventDate = DateTime.Now
            };

            var typeOfCacheableEntity = typeof(Entity);
            var typeOfEntity = typeof(Entity);
            var properties = typeof(TEntity).GetProperties();

            foreach(var prop in properties)
            {
                if(prop.Name == "CreatedAt" || prop.Name == "CreatedBy")
                {
                    continue;
                }

                var propType = prop.PropertyType;

                if(propType.BaseType != null && (propType.BaseType.Equals(typeOfCacheableEntity) || propType.BaseType.Equals(typeOfEntity)))
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

            return entry;
        }

        private IEnumerable<string> GetNavigationPropertiesCanBeUpdated<TEntity>()
        {
            var properties = typeof(TEntity).GetProperties();
            var navPropNames = new HashSet<string>();

            foreach(var prop in properties)
            {
                var attrs = prop.GetCustomAttributes(typeof(LoadOnUpdatingAttribute), true);

                if(attrs is null || attrs.Length == 0)
                {
                    continue;
                }

                navPropNames.Add(prop.Name);
            }

            return navPropNames.ToArray();
        }

        public IReadOnlyList<EntityEventEntry<TEntity>> GetEntityEventEntries<TEntity>() where TEntity : Entity
        {
            var parsed = _entityEventEntries.Cast<EntityEventEntry<TEntity>>().ToArray();

            return parsed;
        }

        public void ClearEntityEventEntries()
        {
            _entityEventEntries.Clear();
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var set = _context.Set<TEntity>();

            set.Attach(entity).State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            _entityEventEntries.Add(new EntityEventEntry<TEntity>()
            {
                Original = entity,
                Updated = null,
                State = Enums.EntityEventState.Deleted,
                EventDate = DateTime.UtcNow
            });
        }

        public async Task<long> InsertAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            var set = _context.Set<TEntity>();

            set.Attach(entity).State = EntityState.Added;

            await _context.SaveChangesAsync();

            _entityEventEntries.Add(new EntityEventEntry<TEntity>()
            {
                Original = entity,
                Updated = null,
                State = Enums.EntityEventState.Added,
                EventDate= DateTime.UtcNow
            });

            return entity.Id;
        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (!_context.Entry<TEntity>(entity).IsKeySet)
            {
                throw new ArgumentException("Entity with no ID set", "entity");
            }

            var navProps = GetNavigationPropertiesCanBeUpdated<TEntity>();
            var set = _context.Set<TEntity>();
            var query = set.AsQueryable();

            if(navProps is not null && navProps.Count() > 0)
            {
                foreach(var navProp in navProps)
                {
                    query = query.Include(navProp);
                }
            }

            var original = await query.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

            if(original == null)
            {
                throw new ArgumentException("Entity not found");
            }

            var entry = AssignUpdatedProps<TEntity>(ref original, ref entity);

            await _context.SaveChangesAsync();

            _entityEventEntries.Add(entry);
        }
    }
}
