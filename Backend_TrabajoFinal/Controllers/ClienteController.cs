using Microsoft.AspNetCore.Mvc;
using Backend_TrabajoFinal.Data;
using Backend_TrabajoFinal.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Backend_TrabajoFinal.Services;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly EstadoFinancieroService _estadoService;

    public ClientesController(AppDbContext context, EstadoFinancieroService estadoService)
    {
        _context = context;
        _estadoService = estadoService;
    }

    [HttpPost]
    public async Task<IActionResult> CrearCliente([FromBody] Cliente cliente)
    {
        if (string.IsNullOrEmpty(cliente.Name) || string.IsNullOrEmpty(cliente.Email))
            return BadRequest("El nombre y el email son obligatorios.");

        // Validación simple de email
        if (!new EmailAddressAttribute().IsValid(cliente.Email))
            return BadRequest("Email inválido");

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return Ok(cliente);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerClientes()
    {
        var clientes = await _context.Clientes
            .Include(c => c.Transacciones) // ✅ carga la colección de transacciones
            .ToListAsync();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerCliente(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Transacciones)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> EditarCliente(int id, [FromBody] Cliente updated)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        if (!string.IsNullOrEmpty(updated.Name))
            cliente.Name = updated.Name;

        if (!string.IsNullOrEmpty(updated.Email))
            cliente.Email = updated.Email;

        await _context.SaveChangesAsync();

        return Ok(cliente);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> BorrarCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/estado")]
    public async Task<IActionResult> ObtenerEstadoCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound("Cliente no encontrado");

        var estado = await _estadoService.ObtenerEstado(id);

        return Ok(estado);
    }
}

//hola