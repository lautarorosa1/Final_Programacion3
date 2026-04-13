namespace Backend_TrabajoFinal.DTOs
{
    public class CrearTransaccionDto
    {
        public string CodigoCripto { get; set; } = "";
        public string TipoTransaccion { get; set; } = "";
        public int ClientId { get; set; }
        public decimal CantidadCripto { get; set; }
        
        // 🔹 Nuevo campo para exchange
        public string? Exchange { get; set; }
    }
}
