using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend_TrabajoFinal.Models
{
    public enum TipoTransaccionEnum
    {
        purchase,
        sale
    }

    public class Transaccion
    {
        public int Id { get; set; }

        public string CodigoCripto { get; set; } = "";

        public TipoTransaccionEnum TipoTransaccion { get; set; }

        public int ClientId { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal CantidadCripto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoARS { get; set; }

        // 🔥 NUEVO: exchange usado
        public string? Exchange { get; set; }

        public DateTime FechaHora { get; set; }
    }
}