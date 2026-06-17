using Microsoft.EntityFrameworkCore;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Statistics;
using Operations.SyntheticDataGenerator;
using ProyectoAnalitica.Dtos;

namespace ProyectoAnalitica.Operations
{
    public class AnalisisCuartaHipotesisOperation
    {
        private readonly UdemyDwContext _context;

        public AnalisisCuartaHipotesisOperation(UdemyDwContext context)
        {
            _context = context;
        }

        public AnalisisH4ResultDto CalcularHipotesis4()
        {
            var resultado = new AnalisisH4ResultDto();

            // 1. Extraer los datos uniendo la tabla de hechos con la dimensión curso
            var datosBase = _context.FactVentasInscripciones
                .Include(f => f.Curso)
                .Select(f => new
                {
                    Precio = (double)f.Curso.PrecioBase,
                    Rating = (double)f.Curso.RatingPromedio,
                    Progreso = (double)f.ProgresoFinalPorcentaje
                })
                .ToList();

            if (!datosBase.Any())
            {
                resultado.Conclusion = "No hay datos suficientes para realizar el análisis.";
                return resultado;
            }

            // 2. Poblar los datos para el Gráfico de Burbujas
            // El radio (r) se multiplica por un factor (ej. 4) para que sea visiblemente óptimo en la interfaz web.
            resultado.BubbleData = datosBase.Select(d => new BubblePointDto
            {
                x = d.Precio,
                y = d.Progreso,
                r = d.Rating * 4 
            }).ToList();

            // 3. Calcular la Matriz de Correlación de Pearson usando MathNet.Numerics
            var listaPrecios = datosBase.Select(d => d.Precio).ToArray();
            var listaRatings = datosBase.Select(d => d.Rating).ToArray();
            var listaProgresos = datosBase.Select(d => d.Progreso).ToArray();

            double corrPrecioRating = Correlation.Pearson(listaPrecios, listaRatings);
            double corrPrecioProgreso = Correlation.Pearson(listaPrecios, listaProgresos);
            double corrRatingProgreso = Correlation.Pearson(listaRatings, listaProgresos);

            // Armar estructura plana de matriz para gráficos tipo Matrix/Heatmap
            var variables = new[] { "Precio", "Rating", "Progreso" };
            var matrizValores = new[,] 
            {
                { 1.0, corrPrecioRating, corrPrecioProgreso },
                { corrPrecioRating, 1.0, corrRatingProgreso },
                { corrPrecioProgreso, corrRatingProgreso, 1.0 }
            };

            for (int i = 0; i < variables.Length; i++)
            {
                for (int j = 0; j < variables.Length; j++)
                {
                    resultado.CorrelationMatrix.Add(new HeatmapCellDto
                    {
                        x = variables[i],
                        y = variables[j],
                        v = Math.Round(matrizValores[i, j], 4)
                    });
                }
            }

            // 4. Regresión Lineal Multivariable utilizando MathNet (Progreso = a + b*Rating + c*Precio)
            // Se prepara el diseño de la matriz: una columna de 1s (Intercepto), columna Rating, columna Precio
            double[][] xData = datosBase.Select(d => new double[] { 1.0, d.Rating, d.Precio }).ToArray();
            double[] yData = listaProgresos;

            // Ajuste por Mínimos Cuadrados Múltiples (Multiple Linear Regression)
            double[] p = MultipleRegression.DirectMethod(xData, yData);

            resultado.Intercept = Math.Round(p[0], 4);
            resultado.CoeficienteRating = Math.Round(p[1], 4);
            resultado.CoeficientePrecio = Math.Round(p[2], 4);

            // Calcular R-Cuadrado de manera manual para validación científica
            double yMedia = yData.Mean();
            double ssTot = yData.Sum(y => Math.Pow(y - yMedia, 2));
            double ssRes = datosBase.Sum(d => Math.Pow(d.Progreso - (p[0] + p[1] * d.Rating + p[2] * d.Precio), 2));
            resultado.RCuadrado = ssTot > 0 ? Math.Round(1 - (ssRes / ssTot), 4) : 0;

            // 5. Conclusión basada en el Coeficiente de correlación intermedio de la Hipótesis original (Rating vs Progreso)
            if (corrRatingProgreso > 0.3)
            {
                resultado.Conclusion = $"Hipótesis Aceptada. Existe una correlación positiva moderada/fuerte ({Math.Round(corrRatingProgreso, 2)}) entre el rating y el progreso académico.";
            }
            else if (corrRatingProgreso > 0)
            {
                resultado.Conclusion = $"Hipótesis Débil. Hay una correlación positiva muy leve ({Math.Round(corrRatingProgreso, 2)}) entre el rating de un curso y el progreso de los alumnos.";
            }
            else
            {
                resultado.Conclusion = "Hipótesis Rechazada. Las valoraciones del curso no demuestran un impacto positivo directo sobre el progreso estudiantil.";
            }

            return resultado;
        }
    }
}