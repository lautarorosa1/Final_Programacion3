namespace Backend_TrabajoFinal.DTOs
{
    public class ResumenCriptoDto
    {
        public string Codigo { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal DineroARS { get; set; }
    }

    public class EstadoFinancieroDto
    {
        public List<ResumenCriptoDto> Criptos { get; set; } = new();
        public decimal TotalARS { get; set; }
    }
}
