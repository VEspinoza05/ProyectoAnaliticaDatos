namespace ProyectoAnalitica.Dtos
{
    public class AnalisisH3ResultDto
    {
        public List<BarraAgrupadaDto> DatosChiCuadrado { get; set; } = new();
        public List<CoeficienteRegresionDto> DatosRegresion { get; set; } = new();
        public double ChiCuadradoEstadistico { get; set; }
        public double PValorChiCuadrado { get; set; }
        public string Conclusion { get; set; }
    }

    public class BarraAgrupadaDto
    {
        public string Dispositivo { get; set; }
        public double PorcentajeFinalizo { get; set; }
        public double PorcentajeAbandono { get; set; }
        public int TotalFinalizo { get; set; }
        public int TotalAbandono { get; set; }
    }

    public class CoeficienteRegresionDto
    {
        public string Variable { get; set; } // Ej: "PC", "Móvil", "Tablet"
        public double OddsRatio { get; set; }
        public double IntervaloConfianzaInferior { get; set; }
        public double IntervaloConfianzaSuperior { get; set; }
    }
}