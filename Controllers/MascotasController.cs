using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO.Mascotas;
using MVC.Domain.servicios.Admin.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinaria.Filters;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    [AdminRol]
    public class MascotasController : Controller
    {
        private readonly IMascotasServicescs _mascotaServices;

        public MascotasController(IMascotasServicescs mascotaServices)
        {
            _mascotaServices = mascotaServices;
        }

        #region Views
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Services
        [HttpGet("GetAllMascotas")]
        public async Task<IActionResult> GetAllMascotas()
        {
            List<MascotasDTO> entity = await _mascotaServices.GetAllMascotasAsync();
            return Ok(entity);
        }

        [HttpGet("GetMascotaById")]
        public async Task<IActionResult> GetMascotaById(int id)
        {
            var mascota = await _mascotaServices.GetMascotaByIdAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }
            return Ok(mascota);
        }

        [HttpPost("AddMascota")]
        public async Task<IActionResult> AddMascota([FromForm] AddMascotasDTO add)
        {
            bool success = await  _mascotaServices.AddMascotasAsync(add);
            return Ok(success);
        }

        [HttpPut("UpdateMascota")]
        public async Task<IActionResult> UpdateMascota( UpdateMascotasDTO update)
        {
            bool success = await _mascotaServices.UpdateMascotaAsync(update);
            return Ok(success);
        }

        [HttpDelete("DeleteMascota")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            bool success = await _mascotaServices.DeleteMascotaAsync(id);
            return Ok(success);
        }
        #endregion
    }
}
