using FluentValidation;                 // <-- ESSENCIAL: essa “caixinha” veio do pacote acima
using RhS.SEG.Application.DTOs.Botoes;

namespace RhS.Api.Validators.SEG
{
    /// <summary>
    /// Valida os dados do formulário de Botão (campos obrigatórios e limites).
    /// Pensa como “regras simples” para não entrar dado bagunçado.
    /// </summary>
    public sealed class BotaoFormValidator : AbstractValidator<BotaoFormDto>
    {
        public BotaoFormValidator()
        {
            RuleFor(x => x.CodigoSistema).NotEmpty().MaximumLength(20);
            RuleFor(x => x.CodigoFuncao).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Acao).NotEmpty().MaximumLength(5);
        }
    }
}
