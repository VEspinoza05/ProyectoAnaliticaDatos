using System;

namespace Operations.SyntheticDataGenerator.Model
{
    public class Dim_Estudiante
    {
        public int Id_Estudiante { get; set; }
        public string Nombre_Completo { get; set; } = string.Empty;
        public string? Pais { get; set; }
        public DateTime Fecha_Registro { get; set; }   
    }

    public class Dim_Curso
    {
        public int Id_Curso { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string? Nivel { get; set; }
        public int? Cantidad_Lecciones { get; set; }
    }

    public class Dim_Suscripcion
    {
        public int Id_Suscripcion { get; set; }
        public string Tipo_Acceso { get; set; } = string.Empty;
        public string? Estado_Suscripcion { get; set; }
    }

    public class Dim_Tiempo
    {
        public int Id_Tiempo { get; set; }
        public DateTime Fecha { get; set; }
        public int Anio { get; set; }
        public int Trimestre { get; set; }
        public int Mes { get; set; }
        public string Nombre_Mes { get; set; } = string.Empty;
        public int Dia { get; set; }
        public string Dia_Semana { get; set; } = string.Empty;
    }
}