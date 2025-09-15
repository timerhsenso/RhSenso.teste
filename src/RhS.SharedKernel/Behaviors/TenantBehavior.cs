using MediatR;
using RhS.SharedKernel.Interfaces;

namespace RhS.SharedKernel.Behaviors;

/// <summary>
/// Behavior para aplicação automática do contexto tenant em comandos
/// </summary>
public class TenantBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ITenantAccessor _tenantAccessor;

    public TenantBehavior(ITenantAccessor tenantAccessor)
    {
        _tenantAccessor = tenantAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Verifica se o request implementa ITenantEntity
        if (request is ITenantEntity tenantEntity)
        {
            // Aplica automaticamente o IdSaas se não estiver definido
            if (tenantEntity.IdSaas == 0)
            {
                tenantEntity.IdSaas = _tenantAccessor.GetTenantId();
            }
        }

        // Verifica se há um tenant válido no contexto
        if (!_tenantAccessor.HasValidTenant())
        {
            throw new InvalidOperationException("Contexto de tenant inválido ou não encontrado.");
        }

        return await next();
    }
}

