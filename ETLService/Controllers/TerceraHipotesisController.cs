using Microsoft.AspNetCore.Mvc;
using ProyectoAnalitica.Operations;
using ProyectoAnalitica.Dtos;

namespace ProyectoAnalitica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiAnalliticController : ControllerBase
    {
        private readonly AnalisisTerceraHipotesisOperation _analisisH3Operation;

        public ApiAnalliticController(AnalisisTerceraHipotesisOperation analisisH3Operation)
        {
            _analisisH3Operation = analisisH3Operation;
        }

        [HttpGet("hipotesis-h3")]
        public async Task<ActionResult<AnalisisH3ResultDto>> GetAnalisisH3()
        {
            try
            {
                var resultado = await _analisisH3Operation.CalcularHipotesisH3Async();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al calcular la hipótesis H3: {ex.Message}");
            }
        }
    }
}