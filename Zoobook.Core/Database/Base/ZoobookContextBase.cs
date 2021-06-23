using System;
using System.Diagnostics;
using System.Linq;
using Zoobook.Shared;
using Microsoft.EntityFrameworkCore;

namespace Zoobook.Core
{
    public class ZoobookContextBase : DbContext
    {
        public ZoobookContextBase(DbContextOptions<ZoobookContextBase> options)
            : base(options)
        {
        }

        public ZoobookContextBase(IDatabaseSetting dbSetting)
            : base(new DbContextOptionsBuilder<DbContext>()
                  .UseSqlServer(dbSetting.ConnectionString())
                  .Options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureNodaTime();
            modelBuilder.UseSnakeCase();
            base.OnModelCreating(modelBuilder);
        }

        public void UpdateDatabaseMigration()
        {
            try
            {
                if (Database.GetPendingMigrations().Any())
                {
                    Database.Migrate();
                    Debug.WriteLine($"{GetType().Name} Successfully migrated the database.");
                }
                else
                {
                    Debug.WriteLine($"{GetType().Name} Database is up to date.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
