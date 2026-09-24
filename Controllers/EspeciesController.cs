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
    public class EspeciesController : Controller
    {
        private readonly IEspecieServices _especieServices;

        public EspeciesController(IEspecieServices especieServices)
        {
            _especieServices = especieServices;
        }

        [AdminRol]
        public IActionResult Index()
        {
            return View();
        }

        [UserRol]
        [HttpGet("GetAllEspecies")]
        public async Task<IActionResult> GetAllEspecies()
        {
            List<EspecieDTO> result = await _especieServices.GetAllEspeciesAsync();
            return Ok(result);
        }

        [AdminRol]
        [HttpPost("AddEspecie")]
        public async Task<IActionResult> AddEspecie([FromBody] AddEspecieDTO add)
        {
            bool success = await _especieServices.AddEspeciesAsync(add);
            return Ok(success);
        }

        [AdminRol]
        [HttpPut("UpdateEspecie")]
        public async Task<IActionResult> UpdateEspecie([FromBody] UpdateEspecieDTO update)
        {
            bool success = await _especieServices.UpdateEspeciesAsync(update);
            return Ok(success);
        }

        [AdminRol]
        [HttpDelete("DeleteEspecie")]
        public async Task<IActionResult> DeleteEspecie(int id)
        {
            bool success = await _especieServices.DeleteEspeciesAsync(id);
            return Ok(success);
        }
    }
}
