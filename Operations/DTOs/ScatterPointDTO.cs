namespace Operations.AnaliticOperations.DTOs
{
    // Formato de coordenadas que Chart.js entiende de forma nativa: { x: valor, y: valor }
    public class ScatterPointDto
    {
        public double X { get; set; } // Minutos Vistos
        public double Y { get; set; } // Porcentaje Progreso
    }
}