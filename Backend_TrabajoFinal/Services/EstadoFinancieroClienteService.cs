using Backend_TrabajoFinal.Data;
using Backend_TrabajoFinal.Models;
using Backend_TrabajoFinal.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend_TrabajoFinal.Services
{
    public class EstadoFinancieroService
    {
        private readonly AppDbContext _context;
        private readonly CriptoYaService _criptoYaService;

        public EstadoFinancieroService(AppDbContext context, CriptoYaService criptoYaService)
        {
            _context = context;
            _criptoYaService = criptoYaService;
        }

        public async Task<EstadoFinancieroDto> ObtenerEstado(int clientId)
        {
            var transacciones = await _context.Transacciones
                .Where(t => t.ClientId == clientId)
                .ToListAsync();

            var agrupadas = transacciones.GroupBy(t => t.CodigoCripto);

            var resultado = new EstadoFinancieroDto();

            foreach (var grupo in agrupadas)
            {
                var crypto = grupo.Key;

                var comprado = grupo
                    .Where(t => t.TipoTransaccion == TipoTransaccionEnum.purchase)
                    .Sum(t => t.CantidadCripto);

                var vendido = grupo
                    .Where(t => t.TipoTransaccion == TipoTransaccionEnum.sale)
                    .Sum(t => t.CantidadCripto);

                var cantidad = comprado - vendido;

                if (cantidad <= 0)
                    continue;

                // 🔥 USAR TODOS LOS EXCHANGES
                var precios = await _criptoYaService.ObtenerPreciosTodos(crypto);

                if (!precios.Any())
                    continue;

                // 🔥 Elegir el mejor precio de venta (bid)
                var mejor = precios.OrderByDescending(p => p.data.bid).First();

                var mejorPrecio = mejor.data.bid;

                var dinero = cantidad * mejorPrecio;

                resultado.Criptos.Add(new ResumenCriptoDto
                {
                    Codigo = crypto,
                    Cantidad = cantidad,
                    DineroARS = dinero
                });

                resultado.TotalARS += dinero;
            }

            return resultado;
        }
    }
}