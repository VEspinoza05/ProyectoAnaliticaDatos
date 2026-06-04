using APPCORE;

namespace Operations.DataGenerator.Entities.Facts
{
    // ========================================================================
    // HECHO: INTERACCIONES DIARIAS
    // ========================================================================
    public class Fact_Interacciones_Diarias: EntityClass
    {
    
        [PrimaryKey(Identity = true)]
        public long? Id_Interaccion_Diaria { get; set; }
        public int? Id_Estudiante { get; set; }
        public int? Id_Curso { get; set; }
        public int? Id_Tiempo { get; set; }
        public decimal? Tiempo_Visualizacion_Minutos { get; set; }
        public int? Cantidad_Reproducciones_Video { get; set; }
        public int? Preguntas_Realizadas { get; set; }
        public int? Respuestas_Dadas { get; set; }
    }

    // ========================================================================
    // HECHO: RENDIMIENTO EVALUACIONES
    // ========================================================================
    public class Fact_Rendimiento_Evaluaciones : EntityClass
    {
        [PrimaryKey(Identity = true)]
        public long? Id_Rendimiento { get; set; }
        public int? Id_Estudiante { get; set; }
        public int? Id_Curso { get; set; }
        public int? Id_Suscripcion { get; set; }
        public int? Id_Tiempo { get; set; }
        public decimal? Calificacion_Final { get; set; }
        public decimal? Quizzes_Completados { get; set; }
        public decimal? Progreso_Actual { get; set; }
        public bool? Curso_Finalizado { get; set; }
        public bool? Es_Abandonado_Temprano { get; set; }
    }
}