using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace Zoobook.Shared
{
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Use Snake Casing ing all entities
        /// </summary>
        public static void UseSnakeCase(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName().ToSnakeCase();
                var schema = entity.GetSchema();
                var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, schema);

                entity.SetTableName(tableName);

                foreach (var property in entity.GetProperties())
                    property.SetColumnName(property.GetColumnName(storeObjectIdentifier).ToSnakeCase());

                foreach (var key in entity.GetKeys())
                    key.SetName(key.GetName().ToSnakeCase());

                foreach (var key in entity.GetForeignKeys())
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
            }
        }

        /// <summary>
        /// Configures Nodatime Data Types
        /// </summary>
        public static void ConfigureNodaTime(this ModelBuilder modelBuilder)
        {
            var instantValueConverter = new ValueConverter<Instant, DateTimeOffset>(
                instant => instant.ToDateTimeOffset(),
                dateTimeOffset => Instant.FromDateTimeOffset(dateTimeOffset));

            var localTimeValueConverter = new ValueConverter<LocalTime, string>(
                localTime => localTime.ToString(Constants.DefaultTimeFormat, null),
                stringTime => NodatimeUtility.ConvertStringToLocalTime(stringTime));

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var offsetDateTimeProperties = entity.ClrType.GetProperties()
                    .Where(clrProperty => clrProperty.PropertyType == typeof(Instant)
                        || clrProperty.PropertyType == typeof(Instant?));
                foreach (var property in offsetDateTimeProperties)
                {
                    var entityTypeBuilder = modelBuilder.Entity(entity.ClrType);
                    entityTypeBuilder
                        .Property(property.Name)
                        .HasConversion(instantValueConverter);
                }

                var offsetTimeProperties = entity.ClrType.GetProperties()
                    .Where(clrProperty => clrProperty.PropertyType == typeof(LocalTime)
                        || clrProperty.PropertyType == typeof(LocalTime?));
                foreach (var property in offsetTimeProperties)
                {
                    var entityTypeBuilder = modelBuilder.Entity(entity.ClrType);
                    entityTypeBuilder
                        .Property(property.Name)
                        .HasConversion(localTimeValueConverter);
                }
            }
        }

    }
}
