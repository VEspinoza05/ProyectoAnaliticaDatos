namespace Operations.SyntheticDataGenerator.Model
{
    public class Fact_Interacciones_Diarias
    {
        public long Id_Interaccion_Diaria { get; set; }
        public int Id_Estudiante { get; set; }
        public int Id_Curso { get; set; }
        public int Id_Tiempo { get; set; }
        public decimal Tiempo_Visualizacion_Minutos { get; set; }
        public int Cantidad_Reproducciones_Video { get; set; }
        public int Preguntas_Realizadas { get; set; }
        public int Respuestas_Dadas { get; set; }
    }

    public class Fact_Rendimiento_Evaluaciones
    {
        public long Id_Rendimiento { get; set; }
        public int Id_Estudiante { get; set; }
        public int Id_Curso { get; set; }
        public int Id_Suscripcion { get; set; }
        public int Id_Tiempo { get; set; }
        public decimal Calificacion_Final { get; set; }
        public int Quizzes_Completados { get; set; }
        public decimal Progreso_Actual_Porcentaje { get; set; }
        public byte Curso_Finalizado { get; set; }       
        public byte Es_Abandono_Temprano { get; set; }   
    }
}