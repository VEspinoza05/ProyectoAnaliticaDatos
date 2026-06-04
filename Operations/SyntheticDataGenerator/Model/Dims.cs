using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APPCORE;

namespace Operations.DataGenerator.Entities.Dimensions
{
    // ========================================================================
    // DIMENSIÓN: ESTUDIANTE
    // ========================================================================
    public class Dim_Estudiante : EntityClass
    {

        [PrimaryKey(Identity = true)]
        public int? Id_Estudiante { get; set; }
        public string? Nombre { get; set; }
        public string? Pais { get; set; }
        public DateTime Fecha_Registro { get; set; }   
    }

    // ========================================================================
    // DIMENSIÓN: CURSO
    // ========================================================================
    public class Dim_Area_Psicoemocional : EntityClass
    {

        [PrimaryKey(Identity = true)]
        public int? Id_Curso { get; set; }
        public string? Titulo { get; set; }
        public string? Categoria { get; set; }
        public string? Nivel { get; set; }
        public int? Cantidad_Lecciones { get; set; }
    }

    // ========================================================================
    // DIMENSIÓN: SUSCRIPCION
    // ========================================================================
    public class Dim_Tipo_Evolucion : EntityClass
    {

        [PrimaryKey(Identity = true)]
        public int? Id_Suscripcion { get; set; }
        public string? Suscripcion { get; set; }
        public string? Estado { get; set; }
    }

    // ========================================================================
    // DIMENSIÓN: TIEMPO
    // ========================================================================
    public class Dim_Tiempo : EntityClass
    {

        [PrimaryKey(Identity = true)]
        public int? Id_Tiempo { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Anio { get; set; }
        public int? Trimestre { get; set; }
        public int? Mes { get; set; }
        public string? Nombre_Mes { get; set; }
        public int? Dia { get; set; }
        public string? Dia_Semana { get; set; }
    }
}