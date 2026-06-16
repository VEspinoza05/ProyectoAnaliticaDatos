using Microsoft.AspNetCore.Mvc;
using ProyectoAnalitica.Dtos;
using ProyectoAnalitica.Operations;
using UdemyAnalytics.Models; // Ajustar a sus namespaces reales

namespace ProyectoAnalitica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrimeraHipotesisController : ControllerBase
    {
        private readonly AnalisisPrimeraHipotesisOperation _operation;

        // ASP.NET Core inyectará automáticamente el DbContext configurado en Program.cs
        public PrimeraHipotesisController(AnalisisPrimeraHipotesisOperation operation)
        {
            _operation = operation;
        }

        [HttpGet("hipotesis1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnalisisH1ResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAnalisisHipotesis1()
        {
            try
            {   
                var resultado = await _operation.CalcularHipotesis1Async();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al calcular regresión de Hipótesis 1: {ex}");
            }
        }
    }
}