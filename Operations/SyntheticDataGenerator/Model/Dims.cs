using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Operations.SyntheticDataGenerator.Model
{
    [Table("dim_estudiante")]
    public class DimEstudiante
    {
        [Key]
        [Column("id_estudiante")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Nosotros controlaremos los IDs correlativos
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
    }

    [Table("dim_curso")]
    public class DimCurso
    {
        [Key]
        [Column("id_curso")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
    }

    [Table("dim_tiempo")]
    public class DimTiempo
    {
        [Key]
        [Column("id_tiempo")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Formato inteligente numérico YYYYMMDD
        public int IdTiempo { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("dia")]
        public int Dia { get; set; }

        [Column("semana")]
        public int Semana { get; set; }

        [Required]
        [StringLength(20)]
        [Column("mes")]
        public string Mes { get; set; } = string.Empty;

        [Column("anio")]
        public int Anio { get; set; }

        [Column("trimestre")]
        public int Trimestre { get; set; }
    }

    [Table("dim_dispositivo")]
    public class DimDispositivo
    {
        [Key]
        [Column("id_dispositivo")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdDispositivo { get; set; }

        [Required]
        [StringLength(50)]
        [Column("tipo_dispositivo")]
        public string TipoDispositivo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("sistema_operativo")]
        public string SistemaOperativo { get; set; } = string.Empty;
    }

    [Table("dim_promocion")]
    public class DimPromocion
    {
        [Key]
        [Column("id_promocion")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
    }
}