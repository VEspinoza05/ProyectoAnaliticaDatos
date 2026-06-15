using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Operations.SyntheticDataGenerator.Model;

namespace UdemyAnalytics.Models
{
    [Table("fact_interacciones_progreso")]
    public class FactInteraccionesProgreso
    {
        [Key]
        [Column("id_interaccion_progreso")]
        public int IdInteraccionProgreso { get; set; }

        [ForeignKey("Estudiante")]
        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }

        [ForeignKey("Curso")]
        [Column("id_curso")]
        public int IdCurso { get; set; }

        [ForeignKey("Tiempo")]
        [Column("id_tiempo")]
        public int IdTiempo { get; set; }

        [ForeignKey("Dispositivo")]
        [Column("id_dispositivo")]
        public int IdDispositivo { get; set; }

        [Column("tiempo_permanencia_segundos")]
        public int TiempoPermanenciaSegundos { get; set; }

        [Column("videos_vistos")]
        public int VideosVistos { get; set; }

        [Column("modulos_completados_count")]
        public int ModulosCompletadosCount { get; set; }

        [Column("porcentaje_progreso_acumulado")]
        public decimal PorcentajeProgresoAcumulado { get; set; }

        // Propiedades de Navegación (Opcionales para EF)
        public required DimEstudiante Estudiante { get; set; }
        public required DimCurso Curso { get; set; }
        public required DimTiempo Tiempo { get; set; }
        public required DimDispositivo Dispositivo { get; set; }
    }

    [Table("fact_evaluaciones")]
    public class FactEvaluaciones
    {
        [Key]
        [Column("id_evaluacion")]
        public int IdEvaluacion { get; set; }

        [ForeignKey("Estudiante")]
        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }

        [ForeignKey("Curso")]
        [Column("id_curso")]
        public int IdCurso { get; set; }

        [ForeignKey("Tiempo")]
        [Column("id_tiempo")]
        public int IdTiempo { get; set; }

        [ForeignKey("Dispositivo")]
        [Column("id_dispositivo")]
        public int IdDispositivo { get; set; }

        [Column("calificacion_obtenida")]
        public decimal CalificacionObtenida { get; set; }

        [Column("intentos_realizados")]
        public int IntentosRealizados { get; set; }

        [Column("aprobado")]
        public int Aprobado { get; set; } // 1 o 0

        public required DimEstudiante Estudiante { get; set; }
        public required DimCurso Curso { get; set; }
        public required DimTiempo Tiempo { get; set; }
        public required DimDispositivo Dispositivo { get; set; }
    }

    [Table("fact_ventas_inscripciones")]
    public class FactVentasInscripciones
    {
        [Key]
        [Column("id_venta_inscripcion")]
        public int IdVentaInscripcion { get; set; }

        [ForeignKey("Estudiante")]
        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }

        [ForeignKey("Curso")]
        [Column("id_curso")]
        public int IdCurso { get; set; }

        [ForeignKey("Tiempo")]
        [Column("id_tiempo")] // Fecha de compra
        public int IdTiempo { get; set; }

        [ForeignKey("Promocion")]
        [Column("id_promocion")]
        public int? IdPromocion { get; set; }

        [ForeignKey("Dispositivo")]
        [Column("id_dispositivo")]
        public int IdDispositivo { get; set; }

        [Column("monto_pagado")]
        public decimal MontoPagado { get; set; }

        [Column("completado")]
        public int Completado { get; set; } // 1 o 0

        [Column("progreso_final_porcentaje")]
        public decimal ProgresoFinalPorcentaje { get; set; }

        [Column("dias_para_terminar")]
        public int? DiasParaTerminar { get; set; } // Nullable si no lo terminó

        public required DimEstudiante Estudiante { get; set; }
        public required DimCurso Curso { get; set; }
        public required DimTiempo Tiempo { get; set; }
        public DimPromocion? Promocion { get; set; }
        public required DimDispositivo Dispositivo { get; set; }
    }
}