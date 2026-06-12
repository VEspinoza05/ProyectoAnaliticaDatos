using Operations.SyntheticDataGenerator.Model;

namespace Operations.SyntheticDataGenerator
{
    public class GeneratorConfig
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TotalEstudiantes { get; set; } = 1000;
        public int Seed { get; set; } = 42;
    }

    public class EstudianteSimulacionModel
    {
        public int IdEstudiante { get; set; }
        public int IdSuscripcion { get; set; }
        public double ProbabilidadCompletar { get; set; } 
    }

    public class SyntheticDataGeneratorOperation
    {
        private readonly GeneratorConfig _config;
        private readonly Random _random;
        private readonly UdemyDwContext _context; // Instancia de EF Core

        private List<Dim_Curso> _cursos = new();
        private List<Dim_Suscripcion> _suscripciones = new();
        private List<EstudianteSimulacionModel> _estudiantesPerfil = new();

        private int _currentEstudianteId = 1;
        private long _currentInteraccionId = 1;
        private long _currentRendimientoId = 1;

        public SyntheticDataGeneratorOperation(GeneratorConfig config)
        {
            _config = config;
            _random = new Random(config.Seed);
            _context = new UdemyDwContext();
        }

        public static async Task Start()
        {
            var startDate = new DateTime(2026, 1, 1);
            var endDate = new DateTime(2026, 3, 31);
            
            var config = new GeneratorConfig
            {
                FechaInicio = startDate,
                FechaFin = endDate,
                TotalEstudiantes = 1000,
                Seed = 42
            };

            using var db = new UdemyDwContext();
            // EF Core verifica si ya corrió el rango de fechas
            var registroExistente = db.Etl_Config.FirstOrDefault(e => e.BeginDate == startDate && e.EndDate == endDate);

            if (registroExistente == null)
            {
                var generator = new SyntheticDataGeneratorOperation(config);
                await generator.EjecutarGeneracionAsync();
                
                db.Etl_Config.Add(new Etl_Config
                {
                    Id = 1,
                    BeginDate = startDate,
                    EndDate = endDate,
                    Update_At = DateTime.Now
                });
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine($"⚠️ Rango [{startDate:yyyy-MM-dd} a {endDate:yyyy-MM-dd}] YA FUE PROCESADO.");
            }
        }

        public async Task EjecutarGeneracionAsync()
        {
            Console.WriteLine("=== INICIANDO GENERADOR CON ENTITY FRAMEWORK CORE ===");

            CargarOPrepararDimensionesFijas();
            GenerarEstudiantesDimension();
            GenerarHechosLogististicos();

            Console.WriteLine("=== PROCESO COMPLETADO Y GUARDADO SIN ERRORES ===");
        }

        private void CargarOPrepararDimensionesFijas()
        {
            Console.WriteLine("--> Configurando dimensiones fijas...");

            // 1. Suscripciones seguras con EF Core
            if (!_context.Dim_Suscripcion.Any(s => s.Id_Suscripcion == 1))
            {
                _context.Dim_Suscripcion.Add(new Dim_Suscripcion { Id_Suscripcion = 1, Tipo_Acceso = "Compra Individual", Estado_Suscripcion = "No Aplica" });
            }
            if (!_context.Dim_Suscripcion.Any(s => s.Id_Suscripcion == 2))
            {
                _context.Dim_Suscripcion.Add(new Dim_Suscripcion { Id_Suscripcion = 2, Tipo_Acceso = "Personal Plan", Estado_Suscripcion = "Activa" });
            }
            _context.SaveChanges(); // Guarda las dimensiones fijas si no existían
            _suscripciones = _context.Dim_Suscripcion.ToList();

            // 2. Cursos seguros con EF Core
            if (!_context.Dim_Curso.Any())
            {
                var categorias = new[] { "Programación", "Negocios", "Diseño", "Data Analytics", "Marketing" };
                var niveles = new[] { "Principiante", "Intermedio", "Avanzado" };

                for (int i = 1; i <= 50; i++)
                {
                    _context.Dim_Curso.Add(new Dim_Curso
                    {
                        Id_Curso = i,
                        Titulo = $"Masterclass en {categorias[i % categorias.Length]} - Vol {i}",
                        Categoria = categorias[i % categorias.Length],
                        Nivel = niveles[i % niveles.Length],
                        Cantidad_Lecciones = _random.Next(12, 60)
                    });
                }
                _context.SaveChanges();
            }
            _cursos = _context.Dim_Curso.ToList();
        }

        private void GenerarEstudiantesDimension()
        {
            var paises = new[] { "Nicaragua", "Costa Rica", "Honduras", "Guatemala", "El Salvador", "Panamá", "México", "Colombia" };
            var nombres = new[] { "Vladimir", "Gilmer", "Fernando", "Alejandro", "María", "Carlos", "Ana", "Luis", "Gabriela", "Laura" };
            var apellidos = new[] { "Espinoza", "Aguirre", "Calderón", "Gomez", "Lopez", "Martinez", "Rodriguez", "Perez" };

            int diasDeRango = (_config.FechaFin - _config.FechaInicio).Days;

            for (int i = 0; i < _config.TotalEstudiantes; i++)
            {
                string nombreCompleto = $"{nombres[_random.Next(nombres.Length)]} {apellidos[_random.Next(apellidos.Length)]}";
                DateTime fRegistro = _config.FechaInicio.AddDays(_random.Next(diasDeRango));

                var estudiante = new Dim_Estudiante
                {
                    Id_Estudiante = _currentEstudianteId,
                    Nombre_Completo = nombreCompleto,
                    Pais = paises[_random.Next(paises.Length)],
                    Fecha_Registro = fRegistro
                };
                
                _context.Dim_Estudiante.Add(estudiante);

                var suscripcionElegida = _suscripciones[_random.Next(_suscripciones.Count)];
                _estudiantesPerfil.Add(new EstudianteSimulacionModel
                {
                    IdEstudiante = _currentEstudianteId,
                    IdSuscripcion = suscripcionElegida.Id_Suscripcion,
                    ProbabilidadCompletar = _random.NextDouble()
                });

                _currentEstudianteId++;
            }
            _context.SaveChanges(); // Guarda todos los estudiantes de un solo golpe
            Console.WriteLine($"✓ Creados {_config.TotalEstudiantes} estudiantes.");
        }

        private void GenerarHechosLogististicos()
        {
            Console.WriteLine("Generando registros transaccionales...");

            foreach (var estudiante in _estudiantesPerfil)
            {
                int cursosInscritos = _random.Next(1, 4);
                var cursosSeleccionados = _cursos.OrderBy(x => _random.Next()).Take(cursosInscritos).ToList();

                foreach (var curso in cursosSeleccionados)
                {
                    bool completaraCurso = estudiante.ProbabilidadCompletar > 0.40; 
                    bool abandonoTemprano = !completaraCurso && estudiante.ProbabilidadCompletar < 0.15; 

                    decimal progresoAcumulado = 0;
                    decimal sumatoriaNotasQuizzes = 0;
                    int totalVideosVistos = 0;
                    decimal totalMinutosVistos = 0;
                    int totalPreguntas = 0;
                    int totalQuizzesRealizados = 0;

                    DateTime fechaCorriente = _config.FechaInicio;

                    while (fechaCorriente <= _config.FechaFin)
                    {
                        if (abandonoTemprano && (fechaCorriente - _config.FechaInicio).TotalDays > 7)
                        {
                            fechaCorriente = fechaCorriente.AddDays(1);
                            continue;
                        }

                        if (progresoAcumulado >= 100)
                        {
                            progresoAcumulado = 100;
                            break;
                        }

                        double ratioInteraccionDiaria = completaraCurso ? 0.35 : 0.10;

                        if (_random.NextDouble() <= ratioInteraccionDiaria)
                        {
                            int idTiempoKey = ObtenerOInsertarTiempo(fechaCorriente);

                            decimal minutosHoy = (decimal)(_random.NextDouble() * (completaraCurso ? 50.0 : 15.0) + 5.0);
                            int videosHoy = _random.Next(1, completaraCurso ? 5 : 3);
                            int preguntasHoy = _random.NextDouble() < (completaraCurso ? 0.12 : 0.02) ? 1 : 0; 
                            int respuestasHoy = preguntasHoy > 0 && _random.NextDouble() < 0.4 ? 1 : 0;

                            _context.Fact_Interacciones_Diarias.Add(new Fact_Interacciones_Diarias
                            {
                                Id_Interaccion_Diaria = _currentInteraccionId,
                                Id_Estudiante = estudiante.IdEstudiante,
                                Id_Curso = curso.Id_Curso,
                                Id_Tiempo = idTiempoKey,
                                Tiempo_Visualizacion_Minutos = Math.Round(minutosHoy, 2),
                                Cantidad_Reproducciones_Video = videosHoy,
                                Preguntas_Realizadas = preguntasHoy,
                                Respuestas_Dadas = respuestasHoy
                            });
                            _currentInteraccionId++;

                            totalMinutosVistos += minutosHoy;
                            totalVideosVistos += videosHoy;
                            totalPreguntas += preguntasHoy;
                            progresoAcumulado += (decimal)(videosHoy * (100.0 / (curso.Cantidad_Lecciones ?? 30)));

                            if (_random.NextDouble() < (completaraCurso ? 0.25 : 0.05))
                            {
                                totalQuizzesRealizados++;
                                decimal notaQuiz = (decimal)(_random.NextDouble() * (completaraCurso ? 30.0 : 50.0) + (completaraCurso ? 70.0 : 40.0));
                                sumatoriaNotasQuizzes += notaQuiz;
                            }
                        }

                        fechaCorriente = fechaCorriente.AddDays(1);
                    }

                    if (progresoAcumulado > 100) progresoAcumulado = 100;
                    decimal notaFinal = totalQuizzesRealizados > 0 ? (sumatoriaNotasQuizzes / totalQuizzesRealizados) : 0;
                    if (notaFinal > 100) notaFinal = 100;

                    bool finalizoCurso = progresoAcumulado >= 85.00m;
                    if (finalizoCurso) progresoAcumulado = 100;

                    int idTiempoCierre = ObtenerOInsertarTiempo(_config.FechaFin);

                    _context.Fact_Rendimiento_Evaluaciones.Add(new Fact_Rendimiento_Evaluaciones
                    {
                        Id_Rendimiento = _currentRendimientoId,
                        Id_Estudiante = estudiante.IdEstudiante,
                        Id_Curso = curso.Id_Curso,
                        Id_Suscripcion = estudiante.IdSuscripcion,
                        Id_Tiempo = idTiempoCierre,
                        Calificacion_Final = Math.Round(notaFinal, 2),
                        Quizzes_Completados = totalQuizzesRealizados,
                        Progreso_Actual_Porcentaje = Math.Round(progresoAcumulado, 2),
                        Curso_Finalizado = (byte)(finalizoCurso ? 1 : 0),
                        Es_Abandono_Temprano = (byte)(abandonoTemprano ? 1 : 0)
                    });
                    _currentRendimientoId++;
                }
            }

            // Guardar absolutamente todo el bloque de millones de hechos de manera transaccional y veloz
            _context.SaveChanges();
        }

        private int ObtenerOInsertarTiempo(DateTime fechaTarget)
        {
            int tiempoKey = (fechaTarget.Year * 10000) + (fechaTarget.Month * 100) + fechaTarget.Day;

            var existe = _context.Dim_Tiempo.FirstOrDefault(t => t.Id_Tiempo == tiempoKey);
            if (existe != null) return tiempoKey;

            var nuevoTiempo = new Dim_Tiempo
            {
                Id_Tiempo = tiempoKey,
                Fecha = fechaTarget.Date,
                Anio = fechaTarget.Year,
                Trimestre = (fechaTarget.Month - 1) / 3 + 1,
                Mes = fechaTarget.Month,
                Nombre_Mes = fechaTarget.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                Dia = fechaTarget.Day,
                Dia_Semana = fechaTarget.ToString("dddd", new System.Globalization.CultureInfo("es-ES"))
            };
            _context.Dim_Tiempo.Add(nuevoTiempo);
            _context.SaveChanges(); // Guardar el tiempo inmediatamente para que esté disponible en el bucle

            return tiempoKey;
        }
    }
}