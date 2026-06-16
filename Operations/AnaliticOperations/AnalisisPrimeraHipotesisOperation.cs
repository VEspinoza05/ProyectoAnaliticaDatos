using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Operations.SyntheticDataGenerator;
using ProyectoAnalitica.Dtos;
using ProyectoAnalitica.Models;

namespace ProyectoAnalitica.Operations
{
    public class AnalisisPrimeraHipotesisOperation
    {
        private readonly MLContext _mlContext;
        private readonly UdemyDwContext _context;

        public AnalisisPrimeraHipotesisOperation(UdemyDwContext context)
        {
            // Inicializamos el contexto de ML.NET
            _mlContext = new MLContext(seed: 42);
            _context = context;
        }

        public async Task<AnalisisH1ResultDto> CalcularHipotesis1Async()
        {
            // 1. Obtener los datos consolidados del Data Warehouse (Simulación de base de datos)
            List<EstudianteH1Data> datosEstudiantes = await ObtenerDatosDeBaseDeDatosAsync();

            // 2. Convertir lista de datos a IDataView de ML.NET
            IDataView dataView = _mlContext.Data.LoadFromEnumerable(datosEstudiantes);

            // 3. Dividir los datos en entrenamiento (80%) y prueba (20%)
            var trainTestSplit = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            // 4. Construir el pipeline de Regresión Logística (LbfgsLogisticRegression)
            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(EstudianteH1Data.PrimerModuloCompletado), nameof(EstudianteH1Data.DiasPrimeraSemana))
                .Append(_mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: nameof(EstudianteH1Data.Completado)));

            // 5. Entrenar el modelo
            var modelo = pipeline.Fit(trainTestSplit.TrainSet);

            // 6. Evaluar la calidad de la predicción con el set de pruebas
            var predicciones = modelo.Transform(trainTestSplit.TestSet);
            var metricas = _mlContext.BinaryClassification.Evaluate(predicciones, labelColumnName: nameof(EstudianteH1Data.Completado));

            // 7. Extraer los coeficientes matemáticos rigurosos de la regresión logística
            var parametrosModelo = modelo.LastTransformer.Model;
            var subModelLineal = parametrosModelo.SubModel as Microsoft.ML.Trainers.LinearBinaryModelParameters;

            double intercepto = 0.0;
            double pesoPrimerModulo = 0.0;
            double pesoDiasPrimeraSemana = 0.0;

            if (subModelLineal != null)
            {
                intercepto = subModelLineal.Bias;
                pesoPrimerModulo = subModelLineal.Weights[0];      // Coeficiente de la variable binaria
                pesoDiasPrimeraSemana = subModelLineal.Weights[1]; // Coeficiente de la variable continua (Días)
            }

            // 8. Construir la Curva Sigmoide teórica (Eje X: Días del 1 al 7)
            // Fijamos el escenario ideal: asumimos que el estudiante "Sí completó el módulo" (1.0f) 
            // para evaluar estrictamente cómo decae su probabilidad biunívoca a medida que pasan los días.
            var curvaSigmoide = new List<SigmoidPointDto>();
            for (int dia = 1; dia <= 7; dia++)
            {
                // Ecuación lineal interna: Z = Intercept + (W1 * Var1) + (W2 * Var2)
                double z = intercepto + (pesoPrimerModulo * 1.0) + (pesoDiasPrimeraSemana * dia);
                
                // Aplicación de la función logística estándar (Sigmoide)
                double probabilidad = 1.0 / (1.0 + Math.Exp(-z));

                curvaSigmoide.Add(new SigmoidPointDto
                {
                    Dia = dia,
                    Probabilidad = Math.Round(probabilidad, 4) // Multiplicar por 100 en el JS si se requiere %
                });
            }

            // 9. Calcular las métricas reales para el Gráfico de Barras Apiladas al 100%
            // Procesamos la lista 'datos' que trajimos de la base de datos (con ToList ya en memoria)
            var grupoSi = datosEstudiantes.Where(d => d.PrimerModuloCompletado >= 1f).ToList();
            var grupoNo = datosEstudiantes.Where(d => d.PrimerModuloCompletado < 1f).ToList();

            var barrasDto = new BarrasApiladasDto
            {
                SiCompletó_Exito = grupoSi.Count(d => d.Completado),
                SiCompletó_Abandono = grupoSi.Count(d => !d.Completado),
                
                NoCompletó_Exito = grupoNo.Count(d => d.Completado),
                NoCompletó_Abandono = grupoNo.Count(d => !d.Completado)
            };

            // 10. Formatear la conclusión de negocio fundamentada
            string conclusion = pesoPrimerModulo > 0
                ? $"Hipótesis Aceptada. El coeficiente de interacción temprana ({pesoPrimerModulo:F2}) es positivo y el impacto del retraso en días ({pesoDiasPrimeraSemana:F2}) confirma que completar el primer módulo en las etapas iniciales de la primera semana maximiza de forma crítica la probabilidad de finalización."
                : $"Hipótesis Rechazada. Las interacciones de la primera semana no muestran un peso predictivo determinante sobre la tasa de éxito final.";

            // 11. Retornar el DTO final estructurado para los gráficos
            return new AnalisisH1ResultDto
            {
                CoeficienteModulo = pesoPrimerModulo,
                CoeficienteDias = pesoDiasPrimeraSemana,
                Intercepto = intercepto,
                Conclusion = conclusion,
                CurvaSigmoide = curvaSigmoide,
                BarrasApiladas100 = barrasDto
            };
        }

        private async Task<List<EstudianteH1Data>> ObtenerDatosDeBaseDeDatosAsync()
        {
            // 1. Obtener las inscripciones base con tipos nullables preventivos directamente en la proyección
            var inscripcionesQuery = _context.FactVentasInscripciones
                .AsNoTracking()
                .Select(v => new
                {
                    v.IdEstudiante,
                    v.IdCurso,
                    // Cast preventivo a int? por si existen nulos físicos en la columna de la BD
                    CompletadoRaw = (int?)v.Completado 
                });

            // 2. Obtener las interacciones de la semana 1 aislando los agregados con tipos explícitamente nullables
            var interaccionesSemana1Query = _context.FactInteraccionesProgreso
                .AsNoTracking()
                .Where(f => f.Tiempo != null && f.Tiempo.Semana == 1) 
                .GroupBy(f => new { f.IdEstudiante, f.IdCurso })
                .Select(g => new
                {
                    g.Key.IdEstudiante,
                    g.Key.IdCurso,
                    // Forzamos nulos válidos en la agregación para que SQL no rompa al mapear
                    MaxModulos = (int?)g.Max(f => (int?)f.ModulosCompletadosCount),
                    MinDia = (int?)g.Min(f => f.Tiempo != null ? (int?)f.Tiempo.Dia : null)
                });

            // 3. Left Join definitivo utilizando coalescencia estricta (operador ??) para limpiar los nulos
            var queryCombinada = from ins in inscripcionesQuery
                                join inter in interaccionesSemana1Query 
                                on new { ins.IdEstudiante, ins.IdCurso } equals new { inter.IdEstudiante, inter.IdCurso } into joinInteracciones
                                from subInter in joinInteracciones.DefaultIfEmpty()
                                select new
                                {
                                    // Manejo seguro del flag de finalización (si es nulo en BD, por defecto será false)
                                    EsCompletado = (ins.CompletadoRaw ?? 0) == 1,
                                    
                                    // Extraemos los valores usando variables intermedias limpias
                                    ModulosSemana1 = subInter != null ? subInter.MaxModulos : null,
                                    DiaSemana1 = subInter != null ? subInter.MinDia : null
                                };

            // 4. Materializamos la lista de objetos anónimos seguros de manera asíncrona
            var datosTemporales = await queryCombinada.ToListAsync();

            // 5. Mapeamos en memoria de la aplicación (LINQ to Objects) hacia el DTO rígido de ML.NET.
            // Al hacerlo aquí, C# ya tiene objetos tangibles y no hay traducción a comandos SQL involucrada.
            var resultadoFinal = datosTemporales.Select(d => new EstudianteH1Data
            {
                Completado = d.EsCompletado,
                
                // Si d.ModulosSemana1 tiene valor y es >= 1, es un 1f, de lo contrario un 0f
                PrimerModuloCompletado = (d.ModulosSemana1.HasValue && d.ModulosSemana1.Value >= 1) ? 1f : 0f,
                
                // Si d.DiaSemana1 tiene valor se usa, si no, se asume el día límite de la semana (7f)
                DiasPrimeraSemana = d.DiaSemana1.HasValue ? (float)d.DiaSemana1.Value : 7f
            }).ToList();

            return resultadoFinal;
        }
    }
}