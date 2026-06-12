using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Operations.AnaliticOperations;
using Operations.AnaliticOperations.Model;

namespace ETLService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiAnaliticController : ControllerBase
    {
        private readonly DimEstudianteOperation _operation;

        public ApiAnaliticController(DimEstudianteOperation operation)
        {
            _operation = operation;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _operation.getStudents();
            return Ok(data);
        }
    }
}