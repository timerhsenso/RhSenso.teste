using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RhS.SharedKernel.Behaviors;
using System.Reflection;

namespace RhS.SharedKernel.Extensions;

/// <summary>
/// Extensões para configuração de serviços
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona os serviços do SharedKernel
    /// </summary>
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        // Adiciona MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Adiciona behaviors do MediatR
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TenantBehavior<,>));

        return services;
    }

    /// <summary>
    /// Adiciona validadores do FluentValidation de um assembly
    /// </summary>
    public static IServiceCollection AddValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }

    /// <summary>
    /// Adiciona MediatR de um assembly específico
    /// </summary>
    public static IServiceCollection AddMediatRFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        return services;
    }
}

