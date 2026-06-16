namespace ProyectoAnalitica.Dtos
{
    public class ScatterPointDto
    {
        public double X { get; set; } // Días transcurridos en la primera semana / Interacción
        public double Y { get; set; } // Probabilidad o Flag de finalización (0 o 1)
        public string Etiqueta { get; set; } = string.Empty;
    }
}