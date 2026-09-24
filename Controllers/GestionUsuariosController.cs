using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO;
using MVC.Data.DTO.User;
using MVC.Domain.servicios.seguridad.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinaria.Filters;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    [SuperAdminRol]
    public class GestionUsuariosController : Controller
    {
        private readonly IUserServices _userServices;

        public GestionUsuariosController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetUsuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            List<UsuarioDTO> users = await _userServices.GetAllUsuariosAsync();
            return Ok(users);
        }

        [HttpGet("GetPropietariosDisponibles")]
        public async Task<IActionResult> GetPropietariosDisponibles()
        {
            var propietarios = await _userServices.GetPropietariosDisponiblesAsync();
            return Ok(propietarios);
        }

        [HttpPost("AddUsuario")]
        public async Task<IActionResult> AddUsuario([FromBody] AddUsuarioAdminDTO add)
        {
            UsuarioCreadoDTO result = await _userServices.AddUsuarioAdminAsync(add);
            return Ok(new ResponseDto { Success = true, Result = result });
        }

        [HttpPut("UpdateEstadoUsuario")]
        public async Task<IActionResult> UpdateEstadoUsuario(Guid id, bool activo)
        {
            bool success = await _userServices.UpdateEstadoUsuarioAsync(id, activo);
            return Ok(success);
        }

        [HttpDelete("DeleteUsuario")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            bool success = await _userServices.DeleteUsuarioAsync(id);
            return Ok(success);
        }
    }
}