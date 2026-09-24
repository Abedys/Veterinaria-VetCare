using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO.Files;
using MVC.Domain.servicios.Admin.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinaria.Filters;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    [UserRol]
    public class FileController : Controller
    {
        private readonly IFileServices _fileServices;

        public FileController(IFileServices fileServices)
        {
            _fileServices = fileServices;
        }

        #region Mascotas Images
        [HttpGet("GetMascotaImages")]
        public async Task<IActionResult> GetMascotaImages(int idMascota)
        {
            List<MascotasImagenesDto> images = await _fileServices.GetMascotaImages(idMascota);
            return Ok(images);
        }

        [HttpDelete("DeleteMascotaImage")]
        public async Task<IActionResult> DeleteMascotaImage(int idFile, int idMascota)
        {
            bool success = await _fileServices.DeleteMascotaImage(idFile, idMascota);
            return Ok(success);
        }

        [HttpPost("AddMascotaImages")]
        [RequestSizeLimit(104857600)] // 100 MB
        public async Task<IActionResult> AddMascotaImages([FromForm] int idMascota, [FromForm] List<IFormFile> fileMascota)
        {
            await _fileServices.AddMascotasImages(new AddMascotaImagesDto
            {
                Id = idMascota,
                Images = fileMascota
            });
            return Ok(true);
        }
        #endregion
    }
}
