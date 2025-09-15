using Microsoft.EntityFrameworkCore;
using RhS.SEG.Application.DTOs.Sistemas;
using RhS.SEG.Application.Interfaces.Sistemas;
using RhS.SEG.Core.Entities;
using RhS.Infrastructure.Data;

namespace RhS.Infrastructure.Services.SEG.Sistemas
{
    public sealed class SistemasService : ISistemasService
    {
        private readonly RhSensoDbContext _db;
        public SistemasService(RhSensoDbContext db) => _db = db;

        public async Task<List<SistemaListDto>> GetAllAsync(CancellationToken ct = default)
            => await _db.Sistemas.AsNoTracking()
                .OrderBy(s => s.DcSistema)
                .Select(s => new SistemaListDto(s.CdSistema, s.DcSistema))
                .ToListAsync(ct);

        public async Task<SistemaListDto?> GetByIdAsync(string codigo, CancellationToken ct = default)
        {
            var s = await _db.Sistemas.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CdSistema == codigo, ct);
            return s is null ? null : new SistemaListDto(s.CdSistema, s.DcSistema);
        }

        public async Task<string> CreateAsync(SistemaCreateDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Codigo) || dto.Codigo.Length > 10)
                throw new ArgumentException("Código inválido (até 10 chars).");
            if (string.IsNullOrWhiteSpace(dto.Descricao) || dto.Descricao.Length > 100)
                throw new ArgumentException("Descrição inválida (até 100 chars).");

            var exists = await _db.Sistemas.AnyAsync(x => x.CdSistema == dto.Codigo, ct);
            if (exists) throw new InvalidOperationException("Código já existente.");

            var entity = new Sistema
            {
                CdSistema = dto.Codigo.Trim(),
                DcSistema = dto.Descricao.Trim(),
                Ativo = true
            };

            _db.Sistemas.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.CdSistema;
        }

        public async Task UpdateAsync(string codigo, SistemaUpdateDto dto, CancellationToken ct = default)
        {
            var s = await _db.Sistemas.FirstOrDefaultAsync(x => x.CdSistema == codigo, ct)
                    ?? throw new KeyNotFoundException("Sistema não encontrado.");

            if (string.IsNullOrWhiteSpace(dto.Descricao) || dto.Descricao.Length > 100)
                throw new ArgumentException("Descrição inválida (até 100 chars).");

            s.DcSistema = dto.Descricao.Trim();
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(string codigo, CancellationToken ct = default)
        {
            var s = await _db.Sistemas.FirstOrDefaultAsync(x => x.CdSistema == codigo, ct)
                    ?? throw new KeyNotFoundException("Sistema não encontrado.");

            _db.Sistemas.Remove(s);
            await _db.SaveChangesAsync(ct);
        }
    }
}
