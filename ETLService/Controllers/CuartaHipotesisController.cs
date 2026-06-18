using Microsoft.AspNetCore.Mvc;
using Operations.DTOs;
using ProyectoAnalitica.Operations;

namespace UdemyAnalitica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuartaHipotesisController : ControllerBase
    {
        private readonly AnalisisCuartaHipotesisOperation _operation;

        public CuartaHipotesisController(AnalisisCuartaHipotesisOperation operation)
        {
            _operation = operation;
        }

        [HttpGet("hipotesis4")]
        public IActionResult GetHipotesis4([FromQuery]DateRangeDTOForFactVI dateRange)
        {
            try
            {
                var parsedDateRangeDTO = dateRange.ToNumericDateRangeDTOFromFactVI();
                var response = _operation.CalcularHipotesis4(parsedDateRangeDTO);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al procesar H4: {ex.Message}");
            }
        }
    }
}