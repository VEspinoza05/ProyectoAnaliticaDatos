using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UdemyAnalytics.Models;

namespace Operations.SyntheticDataGenerator.Model
{
    [Table("dim_estudiante")]
    public class DimEstudiante
    {
        [Key]
        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }

        [Required]
        [StringLength(150)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("pais")]
        public string Pais { get; set; } = string.Empty;

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }
        public List<FactVentasInscripciones> VentasInscripciones { get; set; } = new List<FactVentasInscripciones>();
        public List<FactInteraccionesProgreso> InteraccionesProgresos { get; set; } =  new List<FactInteraccionesProgreso>();
        public List<FactEvaluaciones> Evaluaciones { get; set; } =  new List<FactEvaluaciones>();
    }

    [Table("dim_curso")]
    public class DimCurso
    {
        [Key]
        [Column("id_curso")]
        public int IdCurso { get; set; }

        [Required]
        [StringLength(200)]
        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("nivel")]
        public string Nivel { get; set; } = string.Empty;

        [Column("rating_promedio")]
        public decimal RatingPromedio { get; set; }

        [Column("precio_base")]
        public decimal PrecioBase { get; set; }
        public List<FactVentasInscripciones> VentasInscripciones { get; set; } = new List<FactVentasInscripciones>();
        public List<FactInteraccionesProgreso> InteraccionesProgresos { get; set; } = new List<FactInteraccionesProgreso>();
        public List<FactEvaluaciones> Evaluaciones { get; set; } =  new List<FactEvaluaciones>();
    }

    [Table("dim_tiempo")]
    public class DimTiempo
    {
        [Key]
        [Column("id_tiempo")]
        public int IdTiempo { get; set; } // Formato sugerido: YYYYMMDD

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("dia")]
        public int Dia { get; set; }

        [Column("semana")]
        public int Semana { get; set; }

        [Required]
        [StringLength(20)]
        [Column("mes")]
        public string Mes { get; set; }  = string.Empty;

        [Column("anio")]
        public int Anio { get; set; }

        [Column("trimestre")]
        public int Trimestre { get; set; }
        public List<FactVentasInscripciones> VentasInscripciones { get; set; } = new List<FactVentasInscripciones>();
        public List<FactInteraccionesProgreso> InteraccionesProgresos { get; set; } = new List<FactInteraccionesProgreso>();
        public List<FactEvaluaciones> Evaluaciones { get; set; } = new List<FactEvaluaciones>();
    }

    [Table("dim_dispositivo")]
    public class DimDispositivo
    {
        [Key]
        [Column("id_dispositivo")]
        public int IdDispositivo { get; set; }

        [Required]
        [StringLength(50)]
        [Column("tipo_dispositivo")] // Móvil, PC, Tablet
        public string TipoDispositivo { get; set; }  = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("sistema_operativo")]
        public string SistemaOperativo { get; set; } = string.Empty;
        public List<FactVentasInscripciones> VentasInscripciones { get; set; } = new List<FactVentasInscripciones>();
        public List<FactInteraccionesProgreso> InteraccionesProgresos { get; set; } =  new List<FactInteraccionesProgreso>();
        public List<FactEvaluaciones> Evaluaciones { get; set; } =  new List<FactEvaluaciones>();
    }

    [Table("dim_promocion")]
    public class DimPromocion
    {
        [Key]
        [Column("id_promocion")]
        public int IdPromocion { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre_promocion")]
        public string NombrePromocion { get; set; } = string.Empty;

        [Column("porcentaje_descuento")]
        public decimal PorcentajeDescuento { get; set; }

        [Required]
        [StringLength(50)]
        [Column("tipo_campania")]
        public string TipoCampania { get; set; } = string.Empty;
        public List<FactVentasInscripciones> VentasInscripciones { get; set; } = new List<FactVentasInscripciones>();
    }
}