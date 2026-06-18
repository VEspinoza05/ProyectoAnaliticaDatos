using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MathNet.Numerics.Distributions; // Requiere el paquete MathNet.Numerics
using Operations.SyntheticDataGenerator;
using ProyectoAnalitica.Dtos;
using Operations.DTOs;

namespace ProyectoAnalitica.Operations
{
    public class AnalisisQuintaHipotesisOperation
    {
        private readonly UdemyDwContext _context;

        public AnalisisQuintaHipotesisOperation(UdemyDwContext context)
        {
            _context = context;
        }

        public AnalisisH5ResultDto CalcularHipotesisH5(NumericDateRangeDTO numericDateRangeDTO)
        {
            // 1. Obtener los datos base desde la tabla de hechos principal para H5
            var registrosVentas = _context.FactVentasInscripciones
                .Where(x => x.IdTiempo >= numericDateRangeDTO.Desde && x.IdTiempo <= numericDateRangeDTO.Hasta)
                .Include(f => f.Promocion)
                .Include(f => f.Tiempo)
                .Select(f => new
                {
                    EsPromocion = f.IdPromocion != null && f.Promocion.PorcentajeDescuento > 0,
                    ProgresoFinal = (double)f.ProgresoFinalPorcentaje,
                    Fecha = f.Tiempo.Fecha
                })
                .ToList();

            // Separar universos para ANOVA de una vía (One-way ANOVA)
            var grupoPromo = registrosVentas.Where(r => r.EsPromocion).Select(r => r.ProgresoFinal).ToList();
            var grupoRegular = registrosVentas.Where(r => !r.EsPromocion).Select(r => r.ProgresoFinal).ToList();

            // 2. Ejecutar prueba estadística ANOVA
            var anova = EjecutarAnova(grupoPromo, grupoRegular);

            // 3. Procesar datos para el gráfico de Línea de Tendencia Comparada
            // Agrupamos el progreso final acumulado promedio por día del dataset transaccional
            var tendenciaTemporal = registrosVentas
                .GroupBy(r => r.Fecha.ToString("yyyy-MM-dd"))
                .OrderBy(g => g.Key)
                .Select(g => new ProgresoTemporalDto
                {
                    Fecha = g.Key,
                    PromedioConPromocion = g.Where(r => r.EsPromocion).Select(r => r.ProgresoFinal).DefaultIfEmpty(0).Average(),
                    PromedioPrecioRegular = g.Where(r => !r.EsPromocion).Select(r => r.ProgresoFinal).DefaultIfEmpty(0).Average()
                })
                .ToList();

            // 4. Procesar datos planos mapeados para el gráfico de Violín / Densidad
            var datosDensidad = registrosVentas.Select(r => new DensidadProgresoDto
            {
                Grupo = r.EsPromocion ? "Con Promoción" : "Precio Regular",
                Progreso = r.ProgresoFinal
            }).ToList();

            return new AnalisisH5ResultDto
            {
                AnovaResult = anova,
                TendenciaTemporal = tendenciaTemporal,
                DatosDensidad = datosDensidad
            };
        }

        private AnovaResultDto EjecutarAnova(List<double> grupo1, List<double> grupo2)
        {
            int n1 = grupo1.Count;
            int n2 = grupo2.Count;
            int nTotal = n1 + n2;

            if (n1 < 2 || n2 < 2)
            {
                return new AnovaResultDto { Conclusion = "Datos insuficientes para realizar ANOVA." };
            }

            double avg1 = grupo1.Average();
            double avg2 = grupo2.Average();
            double avgTotal = (grupo1.Sum() + grupo2.Sum()) / nTotal;

            // Suma de cuadrados Entre Grupos (SSB)
            double ssb = (n1 * Math.Pow(avg1 - avgTotal, 2)) + (n2 * Math.Pow(avg2 - avgTotal, 2));
            int dfEntre = 1; // k - 1 grupos
            double msb = ssb / dfEntre;

            // Suma de cuadrados Dentro de Grupos / Error (SSW)
            double ssw = grupo1.Sum(x => Math.Pow(x - avg1, 2)) + grupo2.Sum(x => Math.Pow(x - avg2, 2));
            int dfDentro = nTotal - 2; // N - k grupos
            double msw = ssw / dfDentro;

            // Estadístico F
            double fStat = msb / msw;

            // Encontrar P-Value usando la distribución F de MathNet.Numerics
            double pValue = 1.0;
            try
            {
                var fDist = new FisherSnedecor(dfEntre, dfDentro);
                pValue = 1.0 - fDist.CumulativeDistribution(fStat);
            }
            catch { }

            bool esSignificativo = pValue < 0.05;
            string conclusion = esSignificativo
                ? $"Hipótesis rechazada. Existe una diferencia estadísticamente significativa en el progreso de aprendizaje. Los compradores bajo promoción promedian {avg1:F2}% frente a {avg2:F2}% de precio regular."
                : "Hipótesis aprobada. diferencias estadísticas significativas que demuestren que las promociones disminuyen el compromiso a largo plazo.";

            return new AnovaResultDto
            {
                PromedioConPromocion = avg1,
                PromedioPrecioRegular = avg2,
                FStat = fStat,
                PValue = pValue,
                EsSignificativo = esSignificativo,
                Conclusion = conclusion
            };
        }
    }
}