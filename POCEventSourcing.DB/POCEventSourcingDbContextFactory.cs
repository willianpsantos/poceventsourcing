using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace POCEventSourcing.DB
{
    public class POCEventSourcingDbContextFactory : IDesignTimeDbContextFactory<POCEventSourcingDbContext>
    {
        public POCEventSourcingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<POCEventSourcingDbContext>();
            var connectionString = "Data Source=DESKTOP-VCS41DC\\SQLEXPRESS;Initial Catalog=POCEventSourcing;Persist Security Info=True; MultipleActiveResultSets=True; Integrated Security=true;";

            builder.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly("POCEventSourcing.DB");
            });

            return new POCEventSourcingDbContext(builder.Options);
        }
    }
}
