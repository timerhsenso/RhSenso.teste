using Microsoft.EntityFrameworkCore;
using RhS.SharedKernel.BaseClasses;
using RhS.SharedKernel.Interfaces;
using System.Reflection;
using System.Linq.Expressions;

namespace RhS.Infrastructure.Data;

/// <summary>
/// Contexto principal do banco de dados com suporte a multi-tenancy
/// </summary>
public class RhSensoDbContext : DbContext
{
    private readonly ITenantAccessor _tenantAccessor;

    public RhSensoDbContext(DbContextOptions<RhSensoDbContext> options, ITenantAccessor tenantAccessor)
        : base(options)
    {
        _tenantAccessor = tenantAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica configurações de entidades de todos os assemblies
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Aplica Global Query Filter para multi-tenancy
        ApplyGlobalQueryFilters(modelBuilder);
    }

    /// <summary>
    /// Aplica filtros globais para multi-tenancy em todas as entidades que implementam ITenantEntity
    /// </summary>
    private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            // Verifica se a entidade implementa ITenantEntity
            if (typeof(ITenantEntity).IsAssignableFrom(clrType))
            {
                // Cria o filtro global para o tenant
                var method = typeof(RhSensoDbContext)
                    .GetMethod(nameof(GetTenantFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(clrType);

                var filter = method.Invoke(null, new object[] { _tenantAccessor });
                entityType.SetQueryFilter((LambdaExpression)filter!);
            }
        }
    }

    /// <summary>
    /// Cria o filtro de tenant para uma entidade específica
    /// </summary>
    private static LambdaExpression GetTenantFilter<TEntity>(ITenantAccessor tenantAccessor)
        where TEntity : class, ITenantEntity
    {
        Expression<Func<TEntity, bool>> filter = e => e.IdSaas == tenantAccessor.GetTenantId();
        return filter;
    }

    /// <summary>
    /// Sobrescreve SaveChanges para aplicar auditoria automática
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Sobrescreve SaveChanges para aplicar auditoria automática
    /// </summary>
    public override int SaveChanges()
    {
        ApplyAuditInformation();
        return base.SaveChanges();
    }

    /// <summary>
    /// Aplica informações de auditoria automaticamente
    /// </summary>
    private void ApplyAuditInformation()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        var currentUser = _tenantAccessor.GetCurrentUser();
        var currentTenantId = _tenantAccessor.GetTenantId();
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var auditableEntity = (IAuditableEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                auditableEntity.CriadoEm = now;
                auditableEntity.CriadoPor = currentUser;

                // Aplica IdSaas automaticamente se a entidade for multi-tenant
                if (entry.Entity is ITenantEntity tenantEntity && tenantEntity.IdSaas == 0)
                {
                    tenantEntity.IdSaas = currentTenantId;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                auditableEntity.AtualizadoEm = now;
                auditableEntity.AtualizadoPor = currentUser;

                // Impede alteração dos campos de criação
                entry.Property(nameof(IAuditableEntity.CriadoEm)).IsModified = false;
                entry.Property(nameof(IAuditableEntity.CriadoPor)).IsModified = false;

                // Impede alteração do IdSaas
                if (entry.Entity is ITenantEntity)
                {
                    entry.Property(nameof(ITenantEntity.IdSaas)).IsModified = false;
                }
            }
        }
    }
}

