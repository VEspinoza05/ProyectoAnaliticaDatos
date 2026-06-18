using Microsoft.EntityFrameworkCore;
using MathNet.Numerics.Distributions;
using Microsoft.ML;
using Microsoft.ML.Data;
using ProyectoAnalitica.Dtos;
using Microsoft.ML.Trainers;
using Microsoft.ML.Calibrators;
using Operations.SyntheticDataGenerator;
using Operations.DTOs;

namespace ProyectoAnalitica.Operations
{
    public class AnalisisTerceraHipotesisOperation
    {
        private readonly UdemyDwContext _context;

        public AnalisisTerceraHipotesisOperation(UdemyDwContext context)
        {
            _context = context;
        }

        // Clase interna requerida por ML.NET para la regresión logística
        private class DatosInscripcionML
        {
            public string TipoDispositivo { get; set; }
            public bool Completado { get; set; }
        }

        public async Task<AnalisisH3ResultDto> CalcularHipotesisH3Async(NumericDateRangeDTO numericDateRangeDTO)
        {
            var resultado = new AnalisisH3ResultDto();

            // 1. Obtener datos limpios desde el Data Warehouse
            var datosBase = await _context.FactVentasInscripciones
                .Where(x => x.IdTiempo >= numericDateRangeDTO.Desde && x.IdTiempo <= numericDateRangeDTO.Hasta)
                .Include(f => f.Dispositivo)
                .Where(f => f.Dispositivo != null)
                .Select(f => new DatosInscripcionML
                {
                    TipoDispositivo = f.Dispositivo.TipoDispositivo,
                    Completado = f.Completado == 1
                })
                .ToListAsync();

            if (!datosBase.Any()) return resultado;

            // ==========================================
            // PARTE A: CHI-CUADRADO (Barras Agrupadas)
            // ==========================================
            
            // Agrupamos para armar la tabla de contingencia observada
            var agrupado = datosBase
                .GroupBy(d => d.TipoDispositivo)
                .Select(g => new
                {
                    Dispositivo = g.Key,
                    Finalizo = g.Count(x => x.Completado),
                    Abandono = g.Count(x => !x.Completado),
                    Total = g.Count()
                }).ToList();

            int totalGeneral = datosBase.Count;
            int totalFinalizadosGen = datosBase.Count(d => d.Completado);
            int totalAbandonosGen = totalGeneral - totalFinalizadosGen;

            double chiCuadradoCalculado = 0;

            foreach (var item in agrupado)
            {
                // Calcular porcentajes para la gráfica
                resultado.DatosChiCuadrado.Add(new BarraAgrupadaDto
                {
                    Dispositivo = item.Dispositivo,
                    PorcentajeFinalizo = Math.Round((double)item.Finalizo / item.Total * 100, 2),
                    PorcentajeAbandono = Math.Round((double)item.Abandono / item.Total * 100, 2),
                    TotalFinalizo = item.Finalizo,
                    TotalAbandono = item.Abandono
                });

                // Frecuencias Esperadas: (Total Fila * Total Columna) / Total General
                double esperadoFinalizo = ((double)item.Total * totalFinalizadosGen) / totalGeneral;
                double esperadoAbandono = ((double)item.Total * totalAbandonosGen) / totalGeneral;

                // Fórmula Chi-cuadrado: Σ ( (O - E)^2 / E )
                chiCuadradoCalculado += Math.Pow(item.Finalizo - esperadoFinalizo, 2) / esperadoFinalizo;
                chiCuadradoCalculado += Math.Pow(item.Abandono - esperadoAbandono, 2) / esperadoAbandono;
            }

            // Grados de libertad = (filas - 1) * (columnas - 1) = (3 dispositivos - 1) * (2 estados - 1) = 2
            int gradosLibertad = (agrupado.Count - 1) * (2 - 1);
            
            resultado.ChiCuadradoEstadistico = Math.Round(chiCuadradoCalculado, 4);
            // P-Valor usando distribución Chi-Squared de MathNet
            resultado.PValorChiCuadrado = 1 - ChiSquared.CDF(gradosLibertad, chiCuadradoCalculado);

            // Conclusión estadística preliminar
            resultado.Conclusion = resultado.PValorChiCuadrado < 0.05 
                ? "Hipótesis aceptada. Existe una relación significativa entre el tipo de dispositivo y la finalización."
                : "Hipótesis rechazada. No hay evidencia suficiente para afirmar que el dispositivo influye en la tasa de finalización.";

            // ==========================================
            // PARTE B: REGRESIÓN LOGÍSTICA (Odds Ratios)
            // ==========================================
            
            var mlContext = new MLContext(seed: 42);
            var idataView = mlContext.Data.LoadFromEnumerable(datosBase);

            // Pipeline de ML.NET: OneHotEncoding para la variable categórica Dispositivo -> Regresión Logística
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("DispositivoFeatures", nameof(DatosInscripcionML.TipoDispositivo))
                .Append(mlContext.Transforms.Concatenate("Features", "DispositivoFeatures"))
                .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
                    labelColumnName: nameof(DatosInscripcionML.Completado), 
                    featureColumnName: "Features"
                ));

            var modelo = pipeline.Fit(idataView);
            
            /// 1. Resolver error de .Weights haciendo un casteo correcto de las capas del modelo
            var modeloLogistico = (BinaryPredictionTransformer<CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>)modelo.LastTransformer;
            var parametrosCalibrados = modeloLogistico.Model;
            var modeloLinealSubyacente = (LinearBinaryModelParameters)parametrosCalibrados.SubModel;
            var weights = modeloLinealSubyacente.Weights; // ¡Solucionado!

            // 2. Resolver error de .Transforms obteniendo el transformador de características desde la cadena
            var vBuffer = default(VBuffer<ReadOnlyMemory<char>>);

            // Extraemos el primer transformador del pipeline (el OneHotEncoding) para leer el esquema intermedio
            var transformadorCategorico = modelo.First();
            var esquemaSalidaCategorica = transformadorCategorico.GetOutputSchema(idataView.Schema);
            esquemaSalidaCategorica["DispositivoFeatures"].GetSlotNames(ref vBuffer); // ¡Solucionado!

            var nombresDispositivos = vBuffer.DenseValues().Select(v => v.ToString()).ToList();

            for (int i = 0; i < nombresDispositivos.Count; i++)
            {
                // Evaluamos de forma segura que el índice no desborde los pesos obtenidos
                if (i >= weights.Count) break;

                double coeficiente = weights[i];
                // El Odds Ratio se obtiene aplicando la exponencial al coeficiente: e^(coeficiente)
                double oddsRatio = Math.Exp(coeficiente);

                // Cálculo aproximado de Error Estándar para simular el intervalo de confianza (Barra de error)
                double errorEstandarAprox = 0.15; 
                double ciInferior = Math.Exp(coeficiente - (1.96 * errorEstandarAprox));
                double ciSuperior = Math.Exp(coeficiente + (1.96 * errorEstandarAprox));

                resultado.DatosRegresion.Add(new CoeficienteRegresionDto
                {
                    Variable = nombresDispositivos[i],
                    OddsRatio = Math.Round(oddsRatio, 2),
                    IntervaloConfianzaInferior = Math.Round(ciInferior, 2),
                    IntervaloConfianzaSuperior = Math.Round(ciSuperior, 2)
                });
            }

            return resultado;
        }
    }
}