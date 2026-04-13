namespace Backend_TrabajoFinal.DTOs
{
    public class EditarTransaccionDto
    {
        public decimal? MontoARS { get; set; }
        public decimal? CantidadCripto { get; set; }
        public string? TipoTransaccion { get; set; }
        public DateTime? FechaHora { get; set; }
    }
}