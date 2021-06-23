using Zoobook.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Zoobook.Service.Administration.DataLayer
{
    public class ZoobookAdministrationDbContextFactory : IDesignTimeDbContextFactory<ZoobookAdministrationDbContext>
    {
        public ZoobookAdministrationDbContext CreateDbContext(string[] args)
        {
            // Dummy connection string to generate the migration
            var connectionString = "Integrated Security=true;Pooling=true;";

            // Return the whole db context for configuration
            DbContextOptionsBuilder<ZoobookContextBase> _opt = new();
            _opt.UseSqlServer(connectionString);
            return new ZoobookAdministrationDbContext(_opt.Options);
        }
    }
}
