using Microsoft.AspNetCore.Mvc;
using Backend_TrabajoFinal.Data;
using Backend_TrabajoFinal.Models;
using Microsoft.EntityFrameworkCore;
using Backend_TrabajoFinal.Services;
using Backend_TrabajoFinal.DTOs;


[ApiController]
[Route("api/[controller]")]
public class TransaccionesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly CriptoYaService _criptoYaService;
    private readonly TransaccionService _transaccionService;

    public TransaccionesController(AppDbContext context, CriptoYaService criptoYaService, TransaccionService transaccionService)
    {
        _context = context;
        _criptoYaService = criptoYaService;
        _transaccionService = transaccionService;
    }

    [HttpPost]
    public async Task<IActionResult> CrearTransaccion([FromBody] CrearTransaccionDto dto)
    {
        var result = await _transaccionService.Crear(dto);

        if (!result.ok)
            return BadRequest(result.error);

        return Ok(result.data);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTransacciones([FromQuery] int? clientId)
    {
        var query = _context.Transacciones.AsQueryable();
        if (clientId.HasValue)
            query = query.Where(t => t.ClientId == clientId.Value);

        var lista = await query.OrderByDescending(t => t.FechaHora).ToListAsync();
        return Ok(lista);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerTransaccion(int id)
    {
        var transaccion = await _context.Transacciones.FindAsync(id);
        if (transaccion == null) return NotFound();
        return Ok(transaccion);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> EditarTransaccion(int id, [FromBody] EditarTransaccionDto dto)
    {
        var transaccion = await _context.Transacciones.FindAsync(id);
        if (transaccion == null) return NotFound();

        if (dto.MontoARS.HasValue)
            transaccion.MontoARS = dto.MontoARS.Value;

        if (dto.CantidadCripto.HasValue)
            transaccion.CantidadCripto = dto.CantidadCripto.Value;

        if (dto.FechaHora.HasValue)
            transaccion.FechaHora = dto.FechaHora.Value;

        if (!string.IsNullOrEmpty(dto.TipoTransaccion))
        {
            if (!Enum.TryParse<TipoTransaccionEnum>(dto.TipoTransaccion, true, out var tipo))
                return BadRequest("Tipo de transacción inválido");

            transaccion.TipoTransaccion = tipo;
        }

        await _context.SaveChangesAsync();
        return Ok(transaccion);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> BorrarTransaccion(int id)
    {
        var transaccion = await _context.Transacciones.FindAsync(id);
        if (transaccion == null) return NotFound();

        _context.Transacciones.Remove(transaccion);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("ranking")]
    public async Task<IActionResult> ObtenerRanking(string crypto, string tipo)
    {
        var result = await _criptoYaService.ObtenerRanking(crypto, tipo);

        if (!result.Any())
            return NotFound("No hay datos disponibles.");

        return Ok(result);
    }
}