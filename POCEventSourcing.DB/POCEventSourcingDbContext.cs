using Microsoft.EntityFrameworkCore;
using POCEventSourcing.Entities;

namespace POCEventSourcing.DB
{
    public class POCEventSourcingDbContext : DbContext
    {
        public POCEventSourcingDbContext(DbContextOptions<POCEventSourcingDbContext> options) : base(options)
        {

        }

        public DbSet<Person> Person { get; set; }
        public DbSet<PersonAddress> PersonAddress { get; set; }
    }
}