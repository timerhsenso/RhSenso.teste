using MediatR;
using Microsoft.AspNetCore.Mvc;
using RhS.SharedKernel.Extensions;

namespace RhS.SharedKernel.BaseClasses;

/// <summary>
/// Classe base para todos os controllers do sistema
/// Fornece funcionalidades comuns como acesso ao MediatR e informações do usuário
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtém o ID do tenant atual do usuário logado
    /// </summary>
    protected int GetCurrentTenantId()
    {
        return User.GetTenantId();
    }

    /// <summary>
    /// Obtém o nome do usuário atual
    /// </summary>
    protected string GetCurrentUserName()
    {
        return User.GetUserName();
    }

    /// <summary>
    /// Obtém o ID do usuário atual
    /// </summary>
    protected int GetCurrentUserId()
    {
        return User.GetUserId();
    }

    /// <summary>
    /// Verifica se o usuário tem uma permissão específica
    /// </summary>
    protected bool HasPermission(string permission)
    {
        return User.HasPermission(permission);
    }

    /// <summary>
    /// Retorna uma resposta de sucesso padronizada
    /// </summary>
    protected IActionResult Success(object? data = null, string message = "Operação realizada com sucesso")
    {
        return Ok(new { success = true, message, data });
    }

    /// <summary>
    /// Retorna uma resposta de erro padronizada
    /// </summary>
    protected IActionResult Error(string message, object? errors = null)
    {
        return BadRequest(new { success = false, message, errors });
    }
}

