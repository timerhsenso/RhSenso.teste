using RhS.SEG.Application.DTOs.Sistemas;

namespace RhS.SEG.Application.Interfaces.Sistemas
{
    public interface ISistemasService
    {
        Task<List<SistemaListDto>> GetAllAsync(CancellationToken ct = default);
        Task<SistemaListDto?> GetByIdAsync(string codigo, CancellationToken ct = default);
        Task<string> CreateAsync(SistemaCreateDto dto, CancellationToken ct = default);
        Task UpdateAsync(string codigo, SistemaUpdateDto dto, CancellationToken ct = default);
        Task DeleteAsync(string codigo, CancellationToken ct = default);
    }
}
