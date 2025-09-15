
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RhS.SharedKernel.Interfaces;
using RhS.SharedKernel.Utils.DataTables;
using RhS.SharedKernel.Common.Filters;
using RhS.Web.Infrastructure.EF;
using RhS.SEG.Core.Entities;
using RhS.Api.DTOs.SEG;

namespace RhS.Infrastructure.Services.SEG;

public sealed class UsuarioGridService : IGridService<UsuarioListDto, string>
{
    private readonly DbContext _db;
    private readonly IConfigurationProvider _mapperConfig;

    public UsuarioGridService(DbContext db, IMapper mapper)
    {
        _db = db;
        _mapperConfig = mapper.ConfigurationProvider;
    }

    public async Task<DataTableResponse<UsuarioListDto>> QueryAsync(DataTableRequest req, FilterGroup? filters, CancellationToken ct)
    {
        var query = _db.Set<Usuario>().AsNoTracking().ApplyFilterGroup(filters);
        var totalFiltered = await query.CountAsync(ct);

        if (!string.IsNullOrWhiteSpace(req.SortBy))
        {
            var asc = string.Equals(req.SortDir, "asc", StringComparison.OrdinalIgnoreCase);
            query = req.SortBy.ToLowerInvariant() switch
            {
                "cdusuario"    => asc ? query.OrderBy(x => x.CdUsuario)    : query.OrderByDescending(x => x.CdUsuario),
                "dcusuario"    => asc ? query.OrderBy(x => x.DcUsuario)    : query.OrderByDescending(x => x.DcUsuario),
                "emailusuario" => asc ? query.OrderBy(x => x.EmailUsuario) : query.OrderByDescending(x => x.EmailUsuario),
                "flativo"      => asc ? query.OrderBy(x => x.FlAtivo)      : query.OrderByDescending(x => x.FlAtivo),
                "nouser"       => asc ? query.OrderBy(x => x.NoUser)       : query.OrderByDescending(x => x.NoUser),
                _ => query.OrderBy(x => x.CdUsuario)
            };
        }
        else
        {
            query = query.OrderBy(x => x.CdUsuario);
        }

        var pageSize = Math.Clamp(req.Length <= 0 ? 10 : req.Length, 1, 200);
        var data = await query.Skip(req.Start).Take(pageSize).ProjectTo<UsuarioListDto>(_mapperConfig).ToListAsync(ct);

        return new DataTableResponse<UsuarioListDto>
        {
            Draw = req.Draw,
            RecordsTotal = totalFiltered,
            RecordsFiltered = totalFiltered,
            Data = data
        };
    }

    public async Task<int> BulkDeleteAsync(IEnumerable<string> ids, CancellationToken ct)
    {
        var keys = ids.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        if (keys.Length == 0) return 0;
        var set = _db.Set<Usuario>();
        var toRemove = await set.Where(u => keys.Contains(u.CdUsuario)).ToListAsync(ct);
        set.RemoveRange(toRemove);
        return await _db.SaveChangesAsync(ct);
    }

    public async Task<byte[]> ExportCsvAsync(FilterGroup? filters, CancellationToken ct)
    {
        var query = _db.Set<Usuario>().AsNoTracking().ApplyFilterGroup(filters);
        var rows = await query.OrderBy(x => x.CdUsuario).ProjectTo<UsuarioListDto>(_mapperConfig).ToListAsync(ct);
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("CdUsuario;DcUsuario;EmailUsuario;FlAtivo;NoUser");
        foreach (var r in rows) sb.AppendLine($"{r.CdUsuario};{r.DcUsuario};{r.EmailUsuario};{r.FlAtivo};{r.NoUser}");
        return System.Text.Encoding.UTF8.GetBytes(sb.ToString());
    }

    public Task<byte[]> ExportExcelAsync(FilterGroup? filters, CancellationToken ct) => ExportCsvAsync(filters, ct);
    public Task<byte[]> ExportPdfAsync(FilterGroup? filters, CancellationToken ct) => ExportCsvAsync(filters, ct);

    public async Task<int> ImportAsync(Stream fileStream, string fileName, CancellationToken ct)
    {
        using var sr = new StreamReader(fileStream);
        int imported = 0;
        var set = _db.Set<Usuario>();
        bool header = true;
        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync() ?? "";
            if (header) { header = false; continue; }
            var cols = line.Split(';');
            if (cols.Length < 5) continue;
            var cd = cols[0].Trim();
            if (string.IsNullOrWhiteSpace(cd)) continue;

            var u = await set.FirstOrDefaultAsync(x => x.CdUsuario == cd, ct);
            if (u is null) { u = new Usuario { CdUsuario = cd }; set.Add(u); }
            u.DcUsuario = cols[1].Trim();
            u.EmailUsuario = string.IsNullOrWhiteSpace(cols[2]) ? null : cols[2].Trim();
            u.FlAtivo = string.IsNullOrWhiteSpace(cols[3]) ? "S" : cols[3].Trim();
            if (int.TryParse(cols[4], out var n)) u.NoUser = n;
            imported++;
        }
        await _db.SaveChangesAsync(ct);
        return imported;
    }
}
