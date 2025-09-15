using RhS.SEG.Application.DTOs.Usuarios;

namespace RhS.SEG.Core.Abstractions.SEG.Usuarios
{
    public interface IUsuariosService
    {
        Task<List<UsuarioListDto>> GetAllAsync(bool exibirInativos, CancellationToken ct = default);
        Task<UsuarioListDto?> GetByIdAsync(string codigo, CancellationToken ct = default);
        Task CreateAsync(UsuarioCreateDto dto, CancellationToken ct = default);
        Task UpdateAsync(string codigo, UsuarioUpdateDto dto, CancellationToken ct = default);
        Task DeleteAsync(string codigo, CancellationToken ct = default);
        Task RedefinirSenhaPadraoAsync(IEnumerable<string> codigos, CancellationToken ct = default);
        Task RedefinirSenhaPadraoUsuarioAsync(string codigo, CancellationToken ct = default);
    }
}
