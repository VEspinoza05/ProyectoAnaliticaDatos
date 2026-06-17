namespace ProyectoAnalitica.Dtos
{
    // DTO Principal que el controlador retornará al Frontend
    public class AnalisisH5ResultDto
    {
        public AnovaResultDto AnovaResult { get; set; }
        public List<ProgresoTemporalDto> TendenciaTemporal { get; set; }
        public List<DensidadProgresoDto> DatosDensidad { get; set; }
    }

    public class AnovaResultDto
    {
        public double PromedioConPromocion { get; set; }
        public double PromedioPrecioRegular { get; set; }
        public double FStat { get; set; }
        public double PValue { get; set; }
        public bool EsSignificativo { get; set; }
        public string Conclusion { get; set; }
    }

    public class ProgresoTemporalDto
    {
        public string Fecha { get; set; } // Formato YYYY-MM-DD para el eje X
        public double PromedioConPromocion { get; set; }
        public double PromedioPrecioRegular { get; set; }
    }

    public class DensidadProgresoDto
    {
        public string Grupo { get; set; } // "Con Promoción" o "Precio Regular"
        public double Progreso { get; set; } // Usado para armar el gráfico de Violín/Densidad
    }
}