using Microsoft.EntityFrameworkCore;
using RhS.SharedKernel.Interfaces;

namespace RhS.Infrastructure.Extensions;

/// <summary>
/// Extensões para ModelBuilder do Entity Framework
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configura propriedades padrão para entidades que implementam ITenantEntity
    /// </summary>
    public static void ConfigureTenantEntities(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Adiciona índice no IdSaas para performance
                modelBuilder.Entity(entityType.ClrType)
                    .HasIndex(nameof(ITenantEntity.IdSaas))
                    .HasDatabaseName($"IX_{entityType.GetTableName()}_IdSaas");

                // Configura IdSaas como obrigatório
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(ITenantEntity.IdSaas))
                    .IsRequired();
            }
        }
    }

    /// <summary>
    /// Configura propriedades padrão para entidades que implementam IAuditableEntity
    /// </summary>
    public static void ConfigureAuditableEntities(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                var entity = modelBuilder.Entity(entityType.ClrType);

                // Configura propriedades de auditoria
                entity.Property(nameof(IAuditableEntity.CriadoEm))
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(nameof(IAuditableEntity.CriadoPor))
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(nameof(IAuditableEntity.AtualizadoPor))
                    .HasMaxLength(255);

                entity.Property(nameof(IAuditableEntity.Ativo))
                    .IsRequired()
                    .HasDefaultValue(true);

                // Adiciona índices para performance
                entity.HasIndex(nameof(IAuditableEntity.Ativo))
                    .HasDatabaseName($"IX_{entityType.GetTableName()}_Ativo");

                entity.HasIndex(nameof(IAuditableEntity.CriadoEm))
                    .HasDatabaseName($"IX_{entityType.GetTableName()}_CriadoEm");
            }
        }
    }

    /// <summary>
    /// Configura convenções padrão para todas as entidades
    /// </summary>
    public static void ConfigureConventions(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Configura propriedades string com tamanho padrão
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                {
                    property.SetMaxLength(255);
                }
            }

            // Configura propriedades decimal com precisão padrão
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                {
                    property.SetPrecision(18);
                    property.SetScale(2);
                }
            }
        }
    }
}

