using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MathNet.Numerics.Statistics;
using Operations.AnaliticOperations.Model;
using Operations.AnaliticOperations.DTOs;

namespace Operations.AnaliticOperations
{
    public class AnalisisHipotesisOperation
    {
        private readonly UdemyDwContext _context;

        public AnalisisHipotesisOperation(UdemyDwContext context)
        {
            _context = context;
        }

        public async Task<AnalisisH1ResultDto> CalcularHipotesis1Async()
        {
            // 1. Obtener y agrupar visualizaciones (X) desde la tabla de hechos
            var datosVisualizacion = await _context.Fact_Interacciones_Diarias
                .GroupBy(i => new { i.Id_Estudiante, i.Id_Curso })
                .Select(g => new
                {
                    g.Key.Id_Estudiante,
                    g.Key.Id_Curso,
                    TotalMinutos = (double)g.Sum(i => i.Tiempo_Visualizacion_Minutos)
                })
                .ToListAsync();

            // 2. Obtener progreso (Y) desde la tabla de rendimiento
            var datosRendimiento = await _context.Fact_Rendimiento_Evaluaciones
                .Select(r => new
                {
                    r.Id_Estudiante,
                    r.Id_Curso,
                    Progreso = (double)r.Progreso_Actual_Porcentaje
                })
                .ToListAsync();

            // 3. Cruzar la información en memoria usando LINQ
            var analisisCruze = (from vis in datosVisualizacion
                                 join rend in datosRendimiento 
                                 on new { vis.Id_Estudiante, vis.Id_Curso } equals new { rend.Id_Estudiante, rend.Id_Curso }
                                 select new ScatterPointDto
                                 {
                                     X = vis.TotalMinutos,
                                     Y = rend.Progreso
                                 }).ToList();

            if (!analisisCruze.Any())
            {
                return new AnalisisH1ResultDto { Conclusion = "No hay datos suficientes para procesar la prueba." };
            }

            // 4. Extraer vectores independientes para MathNet
            double[] minutosVistos = analisisCruze.Select(p => p.X).ToArray();
            double[] porcentajeProgreso = analisisCruze.Select(p => p.Y).ToArray();

            // 5. Aplicar la prueba de Pearson y T de Student
            double rPearson = Correlation.Pearson(minutosVistos, porcentajeProgreso);
            int n = minutosVistos.Length;
            
            double tEstadistico = rPearson * Math.Sqrt((n - 2) / (1 - Math.Pow(rPearson, 2)));
            var distStudent = new MathNet.Numerics.Distributions.StudentT(0, 1, n - 2);
            double pValor = 2 * (1 - distStudent.CumulativeDistribution(Math.Abs(tEstadistico)));

            string conclusionText = pValor < 0.05 
                ? "Hipótesis Validada: Existe una relación estadísticamente significativa entre el tiempo de visualización y el progreso."
                : "No se puede rechazar la hipótesis nula: Las variables no muestran una relación estadísticamente significativa.";

            // 6. Retornar el DTO completo listo para convertirse en JSON
            return new AnalisisH1ResultDto
            {
                CoeficientePearson = Math.Round(rPearson, 4),
                PValor = pValor,
                Conclusion = conclusionText,
                PuntosGrafico = analisisCruze // Estos datos alimentarán el scatter plot de ChartJS
            };
        }
    }
}