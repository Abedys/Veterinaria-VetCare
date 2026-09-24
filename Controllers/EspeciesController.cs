using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO.Especies;
using MVC.Domain.servicios.Admin.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinaria.Filters;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    [AdminRol]
    public class EspeciesController : Controller
    {
        private readonly IEspecieServices _especieServices;

        public EspeciesController(IEspecieServices especieServices)
        {
            _especieServices = especieServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllEspecies")]
        public async Task<IActionResult> GetAllEspecies()
        {
            List<EspecieDTO> result = await _especieServices.GetAllEspeciesAsync();
            return Ok(result);
        }

        [HttpPost("AddEspecie")]
        public async Task<IActionResult> AddEspecie([FromBody] AddEspecieDTO add)
        {
            bool success = await _especieServices.AddEspeciesAsync(add);
            return Ok(success);
        }

        [HttpPut("UpdateEspecie")]
        public async Task<IActionResult> UpdateEspecie([FromBody] UpdateEspecieDTO update)
        {
            bool success = await _especieServices.UpdateEspeciesAsync(update);
            return Ok(success);
        }

        [HttpDelete("DeleteEspecie")]
        public async Task<IActionResult> DeleteEspecie(int id)
        {
            bool success = await _especieServices.DeleteEspeciesAsync(id);
            return Ok(success);
        }
    }
}
