namespace Operations.AnaliticOperations.DTOs
{
    public class AnalisisH1ResultDto
    {
        public double CoeficientePearson { get; set; }
        public double PValor { get; set; }
        public string Conclusion { get; set; } = string.Empty;
        public List<ScatterPointDto> PuntosGrafico { get; set; } = new();
    }
}