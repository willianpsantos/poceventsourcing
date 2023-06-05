using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.Interfaces.Repositories;

namespace POCEventSourcing.Repositories
{
    public class PersonRepository : Repository, IPersonRepository
    {
        public PersonRepository(IFullDbEntityStateManager dbManager) : base(dbManager)
        {
            
        }

        public async Task DeleteAsync(long id)
        {
            var person = new Person { Id = id };

            await _dbManager.DeleteAsync(person);
        }

        public async Task<long> InsertAsync(Person entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = 1;

            if(entity.Addresses is not null && entity.Addresses.Count > 0)
            {
                foreach (var address in entity.Addresses)
                {
                    address.CreatedAt = DateTime.UtcNow;
                    address.CreatedBy = 1;
                }
            }

            var id = await _dbManager.InsertAsync(entity);

            return id;
        }

        public async Task UpdateAsync(Person entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = 1;

            if (entity.Addresses is not null && entity.Addresses.Count > 0)
            {
                foreach (var address in entity.Addresses)
                {
                    address.UpdatedAt = DateTime.UtcNow;
                    address.UpdatedBy = 1;
                }
            }

            await _dbManager.UpdateAsync(entity);
        }
    }
}