using Zoobook.Core;
using Microsoft.EntityFrameworkCore;

namespace Zoobook.Service.Administration.DataLayer
{
    public class ZoobookAdministrationDbContext : ZoobookContextBase
    {
        public DbSet<Employee> Employees { get; set; }

        public ZoobookAdministrationDbContext(DbContextOptions<ZoobookContextBase> options)
             : base(options)
        {
        }

        public ZoobookAdministrationDbContext(IDatabaseSetting dbSetting)
            : base(dbSetting)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
