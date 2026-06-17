namespace ProyectoAnalitica.Dtos
{
    // DTO para los puntos individuales del gráfico de burbujas (Regresión)
    public class BubblePointDto
    {
        public double x { get; set; } // precio_base
        public double y { get; set; } // progreso_final_porcentaje
        public double r { get; set; } // rating_promedio (Mapeado al radio)
    }

    // DTO para la matriz del mapa de calor (Correlación)
    public class HeatmapCellDto
    {
        public string x { get; set; } // Variable 1
        public string y { get; set; } // Variable 2
        public double v { get; set; } // Valor de correlación
    }

    // DTO Principal de respuesta para H4
    public class AnalisisH4ResultDto
    {
        public string HipotesisId { get; set; } = "H4";
        public string Descripcion { get; set; } = "Relación entre valoraciones (Rating), precio y progreso estudiantil.";
        public List<BubblePointDto> BubbleData { get; set; } = new List<BubblePointDto>();
        public List<HeatmapCellDto> CorrelationMatrix { get; set; } = new List<HeatmapCellDto>();
        
        // Métricas complementarias de la regresión lineal multivariable (Progreso ~ Rating + Precio)
        public double Intercept { get; set; }
        public double CoeficienteRating { get; set; }
        public double CoeficientePrecio { get; set; }
        public double RCuadrado { get; set; }
        public string Conclusion { get; set; }
    }
}