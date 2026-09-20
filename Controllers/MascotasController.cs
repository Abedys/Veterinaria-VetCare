using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO.Mascotas;
using MVC.Domain.servicios.interfaces;

namespace Veterinaria.Controllers
{
    public class MascotasController : Controller
    {

        private readonly IMascotasServicescs _mascotaServices;


        public MascotasController(IMascotasServicescs mascotaServices)
        {
            _mascotaServices = mascotaServices;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("GetAllMascotas")]
        [Route("GetAllMascotas")]
        public async Task<IActionResult> GetAllMascotas()
        {
            List<MascotasDTO> entity = await _mascotaServices.GetAllMascotasAsync();

            return Ok(entity);
        }


    }
}
