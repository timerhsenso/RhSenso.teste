
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RhS.Api.Controllers.Common;
using RhS.SharedKernel.Interfaces;
using RhS.SharedKernel.Utils.DataTables;
using RhS.Api.DTOs.SEG;
using RhS.SEG.Core.Entities;

namespace RhS.Api.Controllers.SEG;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public sealed class UsuariosController : GridControllerBase<UsuarioListDto, string>
{
    private readonly DbContext _db;
    private readonly IMapper _mapper;

    public UsuariosController(IGridService<UsuarioListDto, string> gridService, DbContext db, IMapper mapper) : base(gridService)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioFormDto>> GetById(string id, CancellationToken ct)
    {
        var u = await _db.Set<Usuario>().AsNoTracking().FirstOrDefaultAsync(x => x.CdUsuario == id, ct);
        if (u is null) return NotFound();
        return _mapper.Map<UsuarioFormDto>(u);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioFormDto dto, CancellationToken ct)
    {
        if (await _db.Set<Usuario>().AnyAsync(x => x.CdUsuario == dto.CdUsuario, ct)) return Conflict($"Usuário {dto.CdUsuario} já existe.");
        var entity = _mapper.Map<Usuario>(dto);
        entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
        await _db.AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = entity.CdUsuario }, new { id = entity.CdUsuario });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UsuarioFormDto dto, CancellationToken ct)
    {
        var entity = await _db.Set<Usuario>().FirstOrDefaultAsync(x => x.CdUsuario == id, ct);
        if (entity is null) return NotFound();
        dto.CdUsuario = entity.CdUsuario; // protege a chave
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var entity = await _db.Set<Usuario>().FirstOrDefaultAsync(x => x.CdUsuario == id, ct);
        if (entity is null) return NotFound();
        _db.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpPost("list")]
    public override Task<IActionResult> List([FromForm] DataTableRequest req, CancellationToken ct) => base.List(req, ct);
    [HttpPost("bulk-delete")]
    public override Task<IActionResult> BulkDelete([FromBody] List<string> ids, CancellationToken ct) => base.BulkDelete(ids, ct);
    [HttpPost("export/csv")]
    public override Task<IActionResult> ExportCsv([FromBody] string? filterJson, CancellationToken ct) => base.ExportCsv(filterJson, ct);
    [HttpPost("export/excel")]
    public override Task<IActionResult> ExportExcel([FromBody] string? filterJson, CancellationToken ct) => base.ExportExcel(filterJson, ct);
    [HttpPost("export/pdf")]
    public override Task<IActionResult> ExportPdf([FromBody] string? filterJson, CancellationToken ct) => base.ExportPdf(filterJson, ct);
    [HttpPost("import")]
    public override Task<IActionResult> Import([FromForm] IFormFile file, CancellationToken ct) => base.Import(file, ct);
}
