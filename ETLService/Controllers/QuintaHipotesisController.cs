using Microsoft.AspNetCore.Mvc;
using Operations.DTOs;
using ProyectoAnalitica.Dtos;
using ProyectoAnalitica.Operations;

namespace UdemyAnalitics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuintaHipotesisController : ControllerBase
    {
        private readonly AnalisisQuintaHipotesisOperation _hipotesisOperation;

        public QuintaHipotesisController(AnalisisQuintaHipotesisOperation hipotesisOperation)
        {
            _hipotesisOperation = hipotesisOperation;
        }

        [HttpGet("hipotesis-h5")]
        public ActionResult<AnalisisH5ResultDto> GetAnalisisH5([FromQuery]DateRangeDTOForFactVI dateRange)
        {
            try
            {
                var parsedDateRangeDTO = dateRange.ToNumericDateRangeDTOFromFactVI();
                var resultado = _hipotesisOperation.CalcularHipotesisH5(parsedDateRangeDTO);
                return Ok(resultado);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al procesar la hipótesis H5: {ex.Message}");
            }
        }
    }
}