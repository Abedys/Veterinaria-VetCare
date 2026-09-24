using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO.Propietario;
using MVC.Domain.servicios.Admin.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinaria.Filters;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    [AdminRol]
    public class PropietariosController : Controller
    {
        #region Properties
        private readonly IPropietariosServices _propietariosServices;
        #endregion

        #region Constructor
        public PropietariosController(IPropietariosServices propietariosServices)
        {
            this._propietariosServices = propietariosServices;
        }
        #endregion

        #region Views
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Services
        [HttpGet("GetAllPropietarios")]
        public async Task<IActionResult> GetAllPropietarios()
        {
            List<PropietarioDTO> entities = await _propietariosServices.GetAllPropietariosAsync();
            return Ok(entities);
        }

        [HttpGet("GetPropietarioById")]
        public async Task<IActionResult> GetPropietarioById(int id)
        {
            var entity = await _propietariosServices.GetPropietarioByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost("AddPropietario")]
        public async Task<IActionResult> AddPropietario([FromBody] AddPropietarioDTO add)
        {
            bool success = await _propietariosServices.AddPropietariosAsync(add);
            return Ok(success);
        }

        [HttpPut("UpdatePropietario")]
        public async Task<IActionResult> UpdatePropietario([FromBody] UpdatePropietarioDTO update)
        {
            bool success = await _propietariosServices.UpdatePropietariosAsync(update);
            return Ok(success);
        }

        [HttpDelete("DeletePropietario")]
        public async Task<IActionResult> DeletePropietario(int id)
        {
            bool success = await _propietariosServices.DeletePropietariosAsync(id);
            return Ok(success);
        }
        #endregion
    }
}