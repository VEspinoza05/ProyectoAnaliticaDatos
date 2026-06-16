# Este es el archivo AnalisisHipotesisOperation.cs
```C#
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
```

# Este es el archivo ApiAnalliticController.cs:
```C#
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Operations.AnaliticOperations;

namespace ETLService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiAnaliticController : ControllerBase
    {
        private readonly AnalisisHipotesisOperation _hipotesisOperation;

        // Inyectamos múltiples operaciones según tus requerimientos
        public ApiAnaliticController(
            AnalisisHipotesisOperation hipotesisOperation)
        {
            _hipotesisOperation = hipotesisOperation;
        }

        // NUEVO ENDPOINT: Devuelve los cálculos de la hipótesis y el arreglo numérico
        [HttpGet]
        public async Task<IActionResult> GetHipotesis1Async()
        {
            var resultado = await _hipotesisOperation.CalcularHipotesis1Async();
            return Ok(resultado);
        }
    }
}
```

# Estos son los DTOs
## ScatterPointDto.cs
```C#
namespace Operations.AnaliticOperations.DTOs
{
    // Formato de coordenadas que Chart.js entiende de forma nativa: { x: valor, y: valor }
    public class ScatterPointDto
    {
        public double X { get; set; } // Minutos Vistos
        public double Y { get; set; } // Porcentaje Progreso
    }
}
```

## AnalisisH1ResultDto.cs
```C#
namespace Operations.AnaliticOperations.DTOs
{
    public class AnalisisH1ResultDto
    {
        public double CoeficientePearson { get; set; }
        public double PValor { get; set; }
        public string Conclusion { get; set; } = string.Empty;
        public List<ScatterPointDto> PuntosGrafico { get; set; } = new();
    }
}
```

## Este es el archivo UdemyDwContext.cs:
```C#
using Microsoft.EntityFrameworkCore;
using Operations.SyntheticDataGenerator.Model;
using UdemyAnalytics.Models;

namespace Operations.SyntheticDataGenerator
{
    public class UdemyDwContext : DbContext
    {
        public DbSet<DimEstudiante> DimEstudiantes { get; set; }
        public DbSet<DimCurso> DimCursos { get; set; }
        public DbSet<DimTiempo> DimTiempos { get; set; }
        public DbSet<DimDispositivo> DimDispositivos { get; set; }
        public DbSet<DimPromocion> DimPromociones { get; set; }
        public DbSet<FactInteraccionesProgreso> FactInteraccionesProgreso { get; set; }
        public DbSet<FactEvaluaciones> FactEvaluaciones { get; set; }
        public DbSet<FactVentasInscripciones> FactVentasInscripciones { get; set; }
        public DbSet<EtlConfig> EtlConfig { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Reemplaza con tu cadena de conexión local a SQL Server
            optionsBuilder.UseSqlServer("Server=localhost;Database=DW_Udemy;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de precisión para tipos Decimal (Evita truncados automáticos)
            modelBuilder.Entity<DimCurso>().Property(c => c.PrecioBase).HasPrecision(18, 2);
            modelBuilder.Entity<DimCurso>().Property(c => c.RatingPromedio).HasPrecision(3, 2);
            modelBuilder.Entity<DimPromocion>().Property(p => p.PorcentajeDescuento).HasPrecision(5, 2);
            modelBuilder.Entity<FactInteraccionesProgreso>().Property(f => f.PorcentajeProgresoAcumulado).HasPrecision(5, 2);
            modelBuilder.Entity<FactEvaluaciones>().Property(f => f.CalificacionObtenida).HasPrecision(5, 2);
            modelBuilder.Entity<FactVentasInscripciones>().Property(f => f.MontoPagado).HasPrecision(18, 2);
            modelBuilder.Entity<FactVentasInscripciones>().Property(f => f.ProgresoFinalPorcentaje).HasPrecision(5, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
```

## Este es el archivo index.html
```HTML
<!DOCTYPE html>
<html lang="es">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Análisis de Hipótesis 2 - Learning Analytics</title>
    
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/4.5.0/chart.umd.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@sgratzl/chartjs-chart-boxplot@4.4.4/build/index.umd.min.js"></script>

    <script defer>
        async function cargarGraficoHipotesis2() {
            try {
                // Petición al controlador expuesto anteriormente
                const respuesta = await fetch('/api/SegundaHipotesis/hipotesis2');
                const datosAnaliticos = await respuesta.json();

                // 1. Renderizar Métricas de Resumen e Interpretación Estadísticas
                document.getElementById("txtPearson").innerText = `Coeficiente R de Pearson: ${datosAnaliticos.CoeficienteCorrelacion}`;
                document.getElementById("txtInterpretacion").innerText = `Interpretación: ${datosAnaliticos.InterpretacionCorrelacion}`;
                document.getElementById("txtAnovaF").innerText = `Estadístico F (ANOVA): ${datosAnaliticos.StatF}`;
                document.getElementById("txtPValue").innerText = `Valor p: ${datosAnaliticos.ValorP} (${datosAnaliticos.EsSignificativo ? "Significativo" : "No Significativo"})`;

                // ==========================================
                // 2. CONFIGURACIÓN DEL GRÁFICO DE DISPERSIÓN (SCATTER)
                // ==========================================
                const ctxScatter = document.getElementById('scatterChart').getContext('2d');
                
                // Mapeamos los datos al formato requerido por Chart.js para Scatter: { x: valor, y: valor }
                const scatterPoints = datosAnaliticos.ScatterData.map(p => ({
                    x: p.X,
                    y: p.Y
                }));

                new Chart(ctxScatter, {
                    type: 'scatter',
                    data: {
                        datasets: [{
                            label: 'Estudiantes (Actividad vs Calificación)',
                            data: scatterPoints,
                            backgroundColor: 'rgba(54, 162, 235, 0.6)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            pointRadius: 4,
                            pointHoverRadius: 6
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: { display: true, text: 'Gráfico de Dispersión: Relación Actividad y Rendimiento' }
                        },
                        scales: {
                            x: {
                                title: { display: true, text: 'Actividad Total (Clicks / Interacciones)' }
                            },
                            y: {
                                title: { display: true, text: 'Calificación Final (0 - 100)' },
                                min: 0,
                                max: 100
                            }
                        }
                    }
                });

                // ==========================================
                // 3. CONFIGURACIÓN DEL GRÁFICO DE CAJAS Y BIGOTES (BOXPLOT)
                // ==========================================
                const ctxBoxplot = document.getElementById('boxplotChart').getContext('2d');

                // Extraemos las etiquetas de las categorías ("Actividad Baja", "Actividad Media", "Actividad Alta")
                const boxplotLabels = datosAnaliticos.BoxplotData.map(b => b.Categoria);

                // Mapeamos los datos al formato requerido por el plugin Boxplot de Chart.js: { min, q1, median, q3, max }
                const boxplotValues = datosAnaliticos.BoxplotData.map(b => ({
                    min: b.Minimo,
                    q1: b.Q1,
                    median: b.Mediana,
                    q3: b.Q3,
                    max: b.Maximo
                }));

                new Chart(ctxBoxplot, {
                    type: 'boxplot', // Tipo de gráfico provisto por el CDN del plugin
                    data: {
                        labels: boxplotLabels,
                        datasets: [{
                            label: 'Distribución de Calificaciones',
                            data: boxplotValues,
                            backgroundColor: 'rgba(255, 99, 132, 0.5)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 2,
                            itemRadius: 0 // Oculta los outliers duplicados ya que el backend computó los bigotes completos
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: { display: true, text: 'Gráfico de Cajas y Bigotes: ANOVA por Segmento de Actividad' },
                            legend: { display: false }
                        },
                        scales: {
                            x: {
                                title: { display: true, text: 'Categoría de Actividad Estudiantil' }
                            },
                            y: {
                                title: { display: true, text: 'Calificaciones Finales' },
                                min: 0,
                                max: 100
                            }
                        }
                    }
                });

            } catch (error) {
                console.error("Error al cargar la analítica de la Hipótesis 2:", error);
                document.getElementById("txtInterpretacion").innerText = "Error al conectar con la API de analítica.";
            }
        }

        // Ejecutar al cargar el documento HTML
        cargarGraficoHipotesis2();
    </script>

    <link rel="stylesheet" href="../style.css">
</head>

<body>
    <div class="container">
        <h1>Análisis de la Hipótesis 2 (H2)</h1>
        <p><em>"Los estudiantes con mayor nivel de actividad presentan un mejor rendimiento académico final."</em></p>
        
        <div class="metrics-panel">
            <h2>Resultados Estadísticos del Servidor</h2>
            <p id="txtPearson" class="numericValue"></p>
            <p id="txtInterpretacion" class="interpretation"></p>
            <hr class="textSeparator">
            <p id="txtAnovaF" class="numericValue"></p>
            <p id="txtPValue" class="numericValueStrong"></p>
        </div>

        <div class="chart-box">
            <h2>1. Prueba de Correlación (Análisis Continuo)</h2>
            <canvas id="scatterChart" width="800" height="400"></canvas>
        </div>

        <div class="chart-box">
            <h2>2. Prueba ANOVA (Análisis Categórico)</h2>
            <canvas id="boxplotChart" width="800" height="400"></canvas>
        </div>
    </div>
</body>

</html>
```

## Este es el archivo style.css
```CSS
body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    margin: 30px;
    background-color: #f8f9fa;
    color: #333;
}
.container {
    max-width: 900px;
    margin: 0 auto;
    background: #fff;
    padding: 25px;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}
.metrics-panel {
    background-color: #e9ecef;
    padding: 15px;
    border-radius: 6px;
    margin-bottom: 25px;
}
.chart-box {
    margin-bottom: 40px;
    padding: 15px;
    border: 1px solid #dee2e6;
    border-radius: 6px;
}
h1 { color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 10px; }
h2 { color: #34495e; font-size: 1.2rem; }

.interpretation {
    font-weight: bold; color: #2980b9; margin: 5px 0;
}

.conclusion {
    font-weight: bold; color: #2980b9; margin: 5px 0;
}

.numericValueStrong { 
    font-weight: 500; margin: 5px 0;
}

.numericValue { 
    margin: 5px 0;
}

.textSeparator {
    border: 0; border-top: 1px solid #ccc; margin: 10px 0;
}
```
