namespace ProyectoAnalitica.Dtos
{

    // DTO que contiene las estadísticas de una caja para el Boxplot
    public class BoxplotCategoryDto
    {
        public string Categoria { get; set; } // "Baja", "Media", "Alta"
        public double Minimo { get; set; }
        public double Q1 { get; set; }
        public double Mediana { get; set; }
        public double Q3 { get; set; }
        public double Maximo { get; set; }
        public int CantidadEstudiantes { get; set; }
    }

    // DTO Final que consume el Endpoint de la API
    public class AnalisisH2ResultDto
    {
        // Métricas de Correlación (Scatter)
        public double CoeficienteCorrelacion { get; set; } // Coeficiente R de Pearson
        public string InterpretacionCorrelacion { get; set; }
        public List<ScatterPointDto> ScatterData { get; set; }

        // Métricas de ANOVA (Boxplot)
        public double StatF { get; set; }
        public double ValorP { get; set; } // P-Value aproximado o marcador de significancia
        public bool EsSignificativo { get; set; }
        public List<BoxplotCategoryDto> BoxplotData { get; set; }
    }
}