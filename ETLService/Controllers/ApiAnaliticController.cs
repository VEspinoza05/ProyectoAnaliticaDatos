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