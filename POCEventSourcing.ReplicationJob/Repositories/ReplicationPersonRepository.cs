using Microsoft.EntityFrameworkCore;
using POCEventSourcing.ReplicationJob.DB;
using POCEventSourcing.ReplicationJob.Entities;
using POCEventSourcing.ReplicationJob.Interfaces;

namespace POCEventSourcing.ReplicationJob.Repositories
{
    internal class ReplicationPersonRepository : ReplicationRepository, IReplicationPersonRepository
    {
        private readonly POCReplicationDbContext _context;

        public ReplicationPersonRepository(POCReplicationDbContext context)
        {
            _context = context;
        }

        internal POCReplicationDbContext Context => _context;

        public async Task DeleteAsync(long id)
        {
            Context.Entry(new Person { Id = id }).State = EntityState.Deleted;

            await Context.SaveChangesAsync();
        }

        public async Task<long> InsertAsync(Person entity)
        {
            var set = Context.Set<Person>();

            set.Add(entity);

            await Context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(Person entity)
        {
            var set = Context.Set<Person>();
            
            var original = await 
                set
                    .Include(x => x.Addresses)
                    .Where(x => x.Id ==  entity.Id)
                    .FirstOrDefaultAsync();

            await Context.SaveChangesAsync();
        }
    }
}
