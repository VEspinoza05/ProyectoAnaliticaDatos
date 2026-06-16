using Microsoft.AspNetCore.Mvc;
using Operations.AnaliticOperations;
using ProyectoAnalitica.Dtos;
using ProyectoAnalitica.Operations;

namespace ProyectoAnalitica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SegundaHipotesisController : ControllerBase
    {
        private readonly AnalisisSegundaHipotesisOperation _hipotesisOperation;

        // Se inyecta la clase operacional mediante el contenedor DI de .NET
        public SegundaHipotesisController(AnalisisSegundaHipotesisOperation hipotesisOperation)
        {
            // En arquitectura limpia real usarías inyección por constructor asignada:
            _hipotesisOperation = hipotesisOperation;
        }

        /// <summary>
        /// Obtiene los cálculos analíticos de la Hipótesis 2 (H2) para la graficación de Correlación y ANOVA.
        /// </summary>
        /// <returns>AnalisisH2ResultDto con arreglos estructurados para Scatter y Boxplot</returns>
        [HttpGet("hipotesis2")]
        [ProducesResponseType(typeof(AnalisisH2ResultDto), 200)]
        public IActionResult GetAnalisisH2()
        {
            try
            {
                var resultado = _hipotesisOperation.CalcularHipotesisH2();
                return Ok(resultado);
            }
            catch (System.Exception ex)
            {
                // Manejo elemental de excepciones
                return StatusCode(500, $"Error interno al procesar los modelos de analítica: {ex.Message}");
            }
        }
    }
}