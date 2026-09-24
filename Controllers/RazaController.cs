using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO.Raza;
using MVC.Domain.servicios.Admin.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinaria.Filters;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class RazaController : Controller
    {
        #region Properties
        private readonly IRazaServices _razaServices;
        #endregion

        #region Constructor
        public RazaController(IRazaServices razaServices)
        {
            this._razaServices = razaServices;
        }
        #endregion

        #region Views
        [AdminRol]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Services
        [UserRol]
        [HttpGet("GetAllRazas")]
        public async Task<IActionResult> GetAllRazas()
        {
            List<RazaDTO> entities = await _razaServices.GetAllRazasAsync();
            return Ok(entities);
        }

        [UserRol]
        [HttpGet("GetRazasByEspecie")]
        public async Task<IActionResult> GetRazasByEspecie(int idEspecie)
        {
            List<RazaDTO> entities = await _razaServices.GetRazasByEspecieAsync(idEspecie);
            return Ok(entities);
        }

        [AdminRol]
        [HttpPost("AddRaza")]
        public async Task<IActionResult> AddRaza([FromBody] AddRazaDTO add)
        {
            bool success = await _razaServices.AddRazaAsync(add);
            return Ok(success);
        }

        [AdminRol]
        [HttpPut("UpdateRaza")]
        public async Task<IActionResult> UpdateRaza([FromBody] UpdateRazaDTO update)
        {
            bool success = await _razaServices.UpdateRazaAsync(update);
            return Ok(success);
        }

        [AdminRol]
        [HttpDelete("DeleteRaza")]
        public async Task<IActionResult> DeleteRaza(int id)
        {
            bool success = await _razaServices.DeleteRazaAsync(id);
            return Ok(success);
        }
        #endregion
    }
}