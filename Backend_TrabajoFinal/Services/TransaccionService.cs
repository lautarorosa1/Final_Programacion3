using Backend_TrabajoFinal.Data;
using Backend_TrabajoFinal.Models;
using Backend_TrabajoFinal.Services;
using Backend_TrabajoFinal.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend_TrabajoFinal.Services
{
    public class TransaccionService
    {
        private readonly AppDbContext _context;
        private readonly CriptoYaService _criptoYaService;

        public TransaccionService(AppDbContext context, CriptoYaService criptoYaService)
        {
            _context = context;
            _criptoYaService = criptoYaService;
        }

        public async Task<(bool ok, string? error, Transaccion? data)> Crear(CrearTransaccionDto dto)
        {
            // 1️⃣ Validaciones básicas
            if (dto.CantidadCripto <= 0)
                return (false, "La cantidad debe ser mayor a 0", null);

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == dto.ClientId);

            if (cliente == null)
                return (false, "Cliente no encontrado", null);

            string crypto = dto.CodigoCripto.ToLower();
            string[] validas = { "btc", "eth", "usdc" };

            if (!validas.Contains(crypto))
                return (false, "Criptomoneda inválida", null);

            if (!Enum.TryParse<TipoTransaccionEnum>(dto.TipoTransaccion, true, out var tipo))
                return (false, "Tipo de transacción inválido", null);

            // 2️⃣ Validar saldo si es venta
            if (tipo == TipoTransaccionEnum.sale)
            {
                var comprado = await _context.Transacciones
                    .Where(t => t.ClientId == dto.ClientId &&
                                t.CodigoCripto == crypto &&
                                t.TipoTransaccion == TipoTransaccionEnum.purchase)
                    .SumAsync(t => t.CantidadCripto);

                var vendido = await _context.Transacciones
                    .Where(t => t.ClientId == dto.ClientId &&
                                t.CodigoCripto == crypto &&
                                t.TipoTransaccion == TipoTransaccionEnum.sale)
                    .SumAsync(t => t.CantidadCripto);

                if (dto.CantidadCripto > (comprado - vendido))
                    return (false, "Saldo insuficiente", null);
            }

            // 3️⃣ Obtener precios de todos los exchanges
            var precios = await _criptoYaService.ObtenerPreciosTodos(crypto);

            if (!precios.Any())
                return (false, "No se pudieron obtener precios", null);

            string exchangeSeleccionado;
            decimal valor;

            // 4️⃣ Determinar exchange y precio
            if (!string.IsNullOrEmpty(dto.Exchange) && precios.Any(p => p.exchange == dto.Exchange))
            {
                // 🔹 Caso: usuario seleccionó un exchange manual
                exchangeSeleccionado = dto.Exchange;
                valor = tipo == TipoTransaccionEnum.purchase
                    ? precios.First(p => p.exchange == dto.Exchange).data.ask
                    : precios.First(p => p.exchange == dto.Exchange).data.bid;
            }
            else
            {
                // 🔹 Caso: usar mejor exchange automáticamente
                if (tipo == TipoTransaccionEnum.purchase)
                {
                    var mejor = precios.OrderBy(p => p.data.ask).First();
                    exchangeSeleccionado = mejor.exchange;
                    valor = mejor.data.ask;
                }
                else
                {
                    var mejor = precios.OrderByDescending(p => p.data.bid).First();
                    exchangeSeleccionado = mejor.exchange;
                    valor = mejor.data.bid;
                }
            }

            // 5️⃣ Crear transacción
            var transaccion = new Transaccion
            {
                CodigoCripto = crypto,
                TipoTransaccion = tipo,
                ClientId = dto.ClientId,
                CantidadCripto = dto.CantidadCripto,
                MontoARS = valor * dto.CantidadCripto,
                Exchange = exchangeSeleccionado,
                FechaHora = DateTime.Now
            };

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return (true, null, transaccion);
        }
    }
}