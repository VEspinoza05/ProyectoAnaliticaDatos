using Microsoft.ML.Data;

namespace ProyectoAnalitica.Models
{
    public class EstudianteH1Data
    {
        [LoadColumn(0)]
        public bool Completado { get; set; } // Variable dependiente (Label)

        [LoadColumn(1)]
        public float PrimerModuloCompletado { get; set; } // 1 si completó, 0 si no

        [LoadColumn(2)]
        public float DiasPrimeraSemana { get; set; } // Días que le tomó interactuar
    }

    public class EstudianteH1Prediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}