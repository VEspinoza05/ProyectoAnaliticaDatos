using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Operations.SyntheticDataGenerator.Model;
using UdemyAnalytics.Models;

namespace Operations.SyntheticDataGenerator
{
    public class GeneratorConfig
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TotalEstudiantes { get; set; } = 1000;
        public int Seed { get; set; } = 42;
    }

    public class PerfilEstudianteSimulado
    {
        public int IdEstudiante { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool EsUsuarioMovil { get; set; }
        public bool CompraConPromocion { get; set; }
        public bool EsMuyActivo { get; set; }
        public bool CompletaModuloTemprano { get; set; }
    }

    public class SyntheticDataGeneratorOperation
    {
        private readonly GeneratorConfig _config;
        private readonly Random _random;
        private readonly UdemyDwContext _context; // Su contexto real de base de datos

        private List<DimCurso> _cursosLocal = new();
        private List<DimTiempo> _tiemposLocal = new();
        private List<DimPromocion> _promocionesLocal = new();
        private List<PerfilEstudianteSimulado> _perfilesEstudiantes = new();

        public SyntheticDataGeneratorOperation(GeneratorConfig config, UdemyDwContext context)
        {
            _config = config;
            _random = new Random(config.Seed);
            _context = context;
        }

        public static async Task Start(UdemyDwContext dbContext)
        {
            var startDate = new DateTime(2026, 1, 1);
            var endDate = new DateTime(2026, 3, 31);

            var config = new GeneratorConfig
            {
                FechaInicio = startDate,
                FechaFin = endDate,
                TotalEstudiantes = 1000
            };

            var yaProcesado = await dbContext.Set<EtlConfig>()
                .AnyAsync(e => e.BeginDate == startDate && e.EndDate == endDate);

            if (!yaProcesado)
            {
                var generator = new SyntheticDataGeneratorOperation(config, dbContext);
                await generator.EjecutarGeneracionAsync();

                dbContext.Set<EtlConfig>().Add(new EtlConfig
                {
                    BeginDate = startDate,
                    EndDate = endDate,
                    Update_At = DateTime.Now
                });
                await dbContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"⚠️ El rango {startDate:yyyy-MM-dd} al {endDate:yyyy-MM-dd} ya cuenta con datos.");
            }
        }

        public async Task EjecutarGeneracionAsync()
        {
            Console.WriteLine("=== GENERACIÓN DE DATOS SINTÉTICOS INICIADA ===");
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            await PoblarDimensionesBaseAsync();
            await GenerarEstudiantesAsync();
            await GenerarHechosAsync();

            _context.ChangeTracker.AutoDetectChangesEnabled = true;
            Console.WriteLine("=== PROCESO COMPLETADO SIN ERRORES ===");
        }

        private async Task PoblarDimensionesBaseAsync()
        {
            // 1. Tiempos
            if (!await _context.DimTiempos.AnyAsync())
            {
                var currDate = _config.FechaInicio;
                while (currDate <= _config.FechaFin)
                {
                    int key = (currDate.Year * 10000) + (currDate.Month * 100) + currDate.Day;
                    _context.DimTiempos.Add(new DimTiempo
                    {
                        IdTiempo = key,
                        Fecha = currDate,
                        Dia = currDate.Day,
                        Semana = (currDate.DayOfYear / 7) + 1,
                        Mes = currDate.ToString("MMMM"),
                        Anio = currDate.Year,
                        Trimestre = (currDate.Month - 1) / 3 + 1
                    });
                    currDate = currDate.AddDays(1);
                }
                await _context.SaveChangesAsync();
            }
            _tiemposLocal = await _context.DimTiempos.ToListAsync();

            // 2. Dispositivos
            if (!await _context.DimDispositivos.AnyAsync())
            {
                _context.DimDispositivos.AddRange(
                    new DimDispositivo { IdDispositivo = 1, TipoDispositivo = "PC", SistemaOperativo = "Windows" },
                    new DimDispositivo { IdDispositivo = 2, TipoDispositivo = "PC", SistemaOperativo = "macOS" },
                    new DimDispositivo { IdDispositivo = 3, TipoDispositivo = "Móvil", SistemaOperativo = "Android" },
                    new DimDispositivo { IdDispositivo = 4, TipoDispositivo = "Móvil", SistemaOperativo = "iOS" }
                );
                await _context.SaveChangesAsync();
            }

            // 3. Promociones
            if (!await _context.DimPromociones.AnyAsync())
            {
                _context.DimPromociones.AddRange(
                    new DimPromocion { IdPromocion = 1, NombrePromocion = "Regular (Sin descuento)", PorcentajeDescuento = 0.00m, TipoCampania = "Ninguna" },
                    new DimPromocion { IdPromocion = 2, NombrePromocion = "Descuento de Temporada", PorcentajeDescuento = 0.50m, TipoCampania = "Estacional" },
                    new DimPromocion { IdPromocion = 3, NombrePromocion = "Cupón Flash", PorcentajeDescuento = 0.70m, TipoCampania = "Marketing" }
                );
                await _context.SaveChangesAsync();
            }
            _promocionesLocal = await _context.DimPromociones.ToListAsync();

            // 4. Cursos
            if (!await _context.DimCursos.AnyAsync())
            {
                var cats = new[] { "Desarrollo", "Diseño", "Data Science", "Negocios" };
                for (int i = 1; i <= 30; i++)
                {
                    _context.DimCursos.Add(new DimCurso
                    {
                        IdCurso = i,
                        Titulo = $"Curso Master Especializado {i}",
                        Categoria = cats[i % cats.Length],
                        Nivel = "Todos los niveles",
                        PrecioBase = _random.Next(30, 150),
                        RatingPromedio = (decimal)(4.0 + (_random.NextDouble() * 1.0))
                    });
                }
                await _context.SaveChangesAsync();
            }
            _cursosLocal = await _context.DimCursos.ToListAsync();
        }

        private async Task GenerarEstudiantesAsync()
        {
            if (!await _context.DimEstudiantes.AnyAsync())
            {
                var paises = new[] { "Nicaragua", "México", "Colombia", "Costa Rica", "España" };
                int totalDias = (_config.FechaFin - _config.FechaInicio).Days;

                for (int i = 1; i <= _config.TotalEstudiantes; i++)
                {
                    var fReg = _config.FechaInicio.AddDays(_random.Next(totalDias / 3)); // Se inscriben al inicio del trimestre
                    _context.DimEstudiantes.Add(new DimEstudiante
                    {
                        IdEstudiante = i,
                        Nombre = $"Estudiante Sintético {i}",
                        Pais = paises[_random.Next(paises.Length)],
                        FechaRegistro = fReg
                    });

                    _perfilesEstudiantes.Add(new PerfilEstudianteSimulado
                    {
                        IdEstudiante = i,
                        FechaRegistro = fReg,
                        EsUsuarioMovil = _random.NextDouble() < 0.35,      // H3: 35% Móviles
                        CompraConPromocion = _random.NextDouble() < 0.60,  // H5: 60% usan cupones
                        EsMuyActivo = _random.NextDouble() < 0.45,         // H2: Nivel de actividad diario
                        CompletaModuloTemprano = _random.NextDouble() < 0.50 // H1: Engagement temprano
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task GenerarHechosAsync()
        {
            Console.WriteLine("--> Inyectando bloques de hechos...");
            int loteTrigger = 0;

            foreach (var perfil in _perfilesEstudiantes)
            {
                int cursosAComprar = _random.Next(1, 3);
                var cursosElegidos = _cursosLocal.OrderBy(_ => _random.Next()).Take(cursosAComprar);

                foreach (var curso in cursosElegidos)
                {
                    // Determinar ID del tiempo de la compra
                    var tiempoCompra = _tiemposLocal.First(t => t.Fecha >= perfil.FechaRegistro);
                    int dispositivoId = perfil.EsUsuarioMovil ? _random.Next(3, 5) : _random.Next(1, 3);
                    int promoId = perfil.CompraConPromocion ? _random.Next(2, 4) : 1;

                    decimal desc = _promocionesLocal.First(p => p.IdPromocion == promoId).PorcentajeDescuento;
                    decimal totalPagado = curso.PrecioBase * (1 - desc);

                    // Hipótesis de abandono cruzada (H3 y H5)
                    bool completaCurso = true;
                    if (perfil.EsUsuarioMovil && _random.NextDouble() < 0.55) completaCurso = false; 
                    if (perfil.CompraConPromocion && _random.NextDouble() < 0.40) completaCurso = false;

                    decimal avanceLimite = completaCurso ? 100.00m : _random.Next(10, 80);

                    // 1. INSERTAR VENTA/INSCRIPCIÓN (Solo IDs primitivos)
                    _context.FactVentasInscripciones.Add(new FactVentasInscripciones
                    {
                        IdEstudiante = perfil.IdEstudiante,
                        IdCurso = curso.IdCurso,
                        IdTiempo = tiempoCompra.IdTiempo,
                        IdDispositivo = dispositivoId,
                        IdPromocion = promoId,
                        MontoPagado = Math.Round(totalPagado, 2),
                        Completado = completaCurso ? 1 : 0,
                        ProgresoFinalPorcentaje = avanceLimite,
                        DiasParaTerminar = completaCurso ? _random.Next(10, 45) : null
                    });

                    // 2. SIMULAR INTERACCIONES DIARIAS 
                    var fechaIterador = tiempoCompra.Fecha;
                    decimal progresoAcumulado = 0;
                    int diaDeEstudio = 0;

                    while (fechaIterador <= _config.FechaFin && progresoAcumulado < avanceLimite)
                    {
                        diaDeEstudio++;
                        bool estudiaHoy = _random.NextDouble() < (perfil.EsMuyActivo ? 0.70 : 0.25);
                        if (diaDeEstudio <= 7 && perfil.CompletaModuloTemprano) estudiaHoy = true; // Forzar H1

                        if (estudiaHoy)
                        {
                            var tiempoActual = _tiemposLocal.FirstOrDefault(t => t.Fecha == fechaIterador);
                            if (tiempoActual != null)
                            {
                                int videos = _random.Next(1, perfil.EsMuyActivo ? 5 : 3);
                                int modulos = (diaDeEstudio <= 7 && perfil.CompletaModuloTemprano && diaDeEstudio % 3 == 0) ? 1 : 0;
                                
                                progresoAcumulado += (videos * 3m) + (modulos * 15m);
                                if (progresoAcumulado > avanceLimite) progresoAcumulado = avanceLimite;

                                _context.FactInteraccionesProgreso.Add(new FactInteraccionesProgreso
                               {
                                   IdEstudiante = perfil.IdEstudiante,
                                   IdCurso = curso.IdCurso,
                                   IdTiempo = tiempoActual.IdTiempo,
                                   IdDispositivo = dispositivoId,
                                   TiempoPermanenciaSegundos = _random.Next(200, 2500),
                                   VideosVistos = videos,
                                   ModulosCompletadosCount = modulos,
                                   PorcentajeProgresoAcumulado = Math.Round(progresoAcumulado, 2)
                               });

                               // 3. SIMULAR EVALUACIONES ALEATORIAS (H2)
                               if (_random.NextDouble() < 0.20)
                               {
                                   decimal nota = (perfil.EsMuyActivo ? 75m : 55m) + (decimal)(_random.NextDouble() * 25);
                                   _context.FactEvaluaciones.Add(new FactEvaluaciones
                                   {
                                       IdEstudiante = perfil.IdEstudiante,
                                       IdCurso = curso.IdCurso,
                                       IdTiempo = tiempoActual.IdTiempo,
                                       IdDispositivo = dispositivoId,
                                       CalificacionObtenida = Math.Round(nota, 2),
                                       IntentosRealizados = _random.Next(1, 3),
                                       Aprobado = nota >= 70m ? 1 : 0
                                   });
                               }
                            }
                        }
                        fechaIterador = fechaIterador.AddDays(_random.Next(1, 3));
                    }

                    // Flush de memoria periódico cada 100 alumnos procesados
                    loteTrigger++;
                    if (loteTrigger % 100 == 0)
                    {
                        await _context.SaveChangesAsync();
                        _context.ChangeTracker.Clear(); // Mantiene la RAM completamente vacía y ligera
                    }
                }
            }
            
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
    }
}