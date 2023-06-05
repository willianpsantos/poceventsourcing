using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.Interfaces.Repositories;

namespace POCEventSourcing.Repositories
{
    public class PersonAddressRepository : Repository, IPersonAddressRepository
    {
        public PersonAddressRepository(IFullDbEntityStateManager dbManager) : base(dbManager)
        {
            _dbManager
                .WriteOnReadableDatabase(false)
                .WriteOnCacheDatabase(false);
        }

        public async Task DeleteAsync(long id)
        {
            var person = new PersonAddress { Id = id };

            await _dbManager.DeleteAsync(person);
        }

        public async Task<long> InsertAsync(PersonAddress entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = 1;

            var id = await _dbManager.InsertAsync(entity);

            return id;
        }

        public async Task UpdateAsync(PersonAddress entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = 1;

            await _dbManager.UpdateAsync(entity);
        }
    }
}