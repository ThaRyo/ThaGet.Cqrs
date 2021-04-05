using System;
using System.Linq;
using System.Reflection;
using ThaGet.Cqrs.Domain.Attributes;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaGet.Shared.Extensions;

namespace ThaGet.Cqrs.Domain.EntityFramework.Persistence
{
    internal static class ModelCreator
    {
        public static string DomainAssemblyName;

        /// <summary>
        /// Creates the EF model by using all <see cref="Entity"/> derived classes.
        /// Supports the following custom attributes:
        /// <see cref="UniqueAttribute"/>
        /// <see cref="Attributes.OwnedAttribute"/> / <see cref="IsOwnedAttribute"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ConfigureEntities(ModelBuilder modelBuilder)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            // TODO Test if force load can be ommitted when there is an entity in PROJECT_NAME.Domain
            // Force load the assembly since it isn't loaded at the time yet
            var entityAssembly = Assembly.Load(DomainAssemblyName);

            var baseEntity = typeof(IEntity<>);
            var entitiesTypeInfos = entityAssembly.DefinedTypes.Where(x => x.AsType() != baseEntity && x.AsType().IsAssignableTo(baseEntity) && !x.IsAbstract);

            foreach (var entityTypeInfo in entitiesTypeInfos)
            {
                // Configure all Entity derived non abstract classes
                ConfigureEntity(modelBuilder, entityTypeInfo);
            }
        }

        /// <summary>
        /// Configure a single entity
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="entityTypeInfo"></param>
        private static void ConfigureEntity(ModelBuilder modelBuilder, TypeInfo entityTypeInfo)
        {
            var entity = GetOrAddEntityType(modelBuilder, entityTypeInfo);
            var entityBuilder = modelBuilder.Entity(entityTypeInfo.AsType());

            ConfigureForeignKeys(entity);
            ApplyUniqueAttribute(entityTypeInfo, entityBuilder);
        }

        private static IMutableEntityType GetOrAddEntityType(ModelBuilder modelBuilder, TypeInfo entityTypeInfo)
        {
            var entity = modelBuilder.Model.FindEntityType(entityTypeInfo.AsType());
            if (entity != null)
            {
                return entity;
            }

            return modelBuilder.Model.AddEntityType(entityTypeInfo.AsType());
        }

        /// <summary>
        /// Configure the foreign keys of a single entity.
        /// Disable Cascade Delete
        /// </summary>
        /// <param name="entity"></param>
        private static void ConfigureForeignKeys(IMutableEntityType entity)
        {
            foreach (var foreignKey in entity.GetForeignKeys().Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        /// <summary>
        /// Create unique indexes
        /// </summary>
        /// <param name="entityTypeInfo"></param>
        /// <param name="entityBuilder"></param>
        private static void ApplyUniqueAttribute(TypeInfo entityTypeInfo, EntityTypeBuilder entityBuilder)
        {
            var uniqueAttributes = entityTypeInfo.DeclaredProperties
                .SelectMany(prop => prop.GetCustomAttributes<UniqueAttribute>().Select(attr => new { PropertyName = prop.Name, Attr = attr }))
                .ToList();

            foreach (var item in uniqueAttributes)
            {
                if (item.Attr.IndexGroup == null)
                    item.Attr.IndexGroup = item.PropertyName;
            }

            foreach (var group in uniqueAttributes.GroupBy(x => x.Attr.IndexGroup))
            {
                entityBuilder.HasIndex(group.Select(x => x.PropertyName).ToArray()).IsUnique();
            }
        }

        public static void SpecifyAllDateTimeToUtc(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTime>(property.Name)
                            .HasConversion(
                                v => v.ToUniversalTime(),
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTime?>(property.Name)
                            .HasConversion(
                                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
                    }
                }
            }
        }
    }
}
