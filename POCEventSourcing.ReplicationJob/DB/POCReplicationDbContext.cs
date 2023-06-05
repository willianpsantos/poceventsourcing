using Microsoft.EntityFrameworkCore;
using POCEventSourcing.ReplicationJob.Entities;

namespace POCEventSourcing.ReplicationJob.DB
{
    internal class POCReplicationDbContext : DbContext
    {
        public POCReplicationDbContext(DbContextOptions<POCReplicationDbContext> options): base(options)
        {

        }

        public DbSet<Person> Person { get; set; }
        public DbSet<PersonAddress> PersonAddress { get; set; }
    }
}
