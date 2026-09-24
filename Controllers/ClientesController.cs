using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO;
using MVC.Data.DTO.Mascotas;
using MVC.Data.DTO.User;
using MVC.Domain.servicios.Clientes.interfaces;
using MVC.Domain.servicios.seguridad.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinaria.Filters;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    [ClienteRol]
    public class ClientesController : Controller
    {
        private readonly IClienteServices _clienteServices;
        private readonly IUserSessionServices _userSessionServices;

        public ClientesController(IClienteServices clienteServices, IUserSessionServices userSessionServices)
        {
            _clienteServices = clienteServices;
            _userSessionServices = userSessionServices;
        }

        #region Views

        public IActionResult Mascotas()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            return View();
        }

        #endregion

        #region Perfil

        [HttpGet("GetPerfilCliente")]
        public async Task<IActionResult> GetPerfilCliente()
        {
            Guid? userId = _userSessionServices.GetCurrentUserId();
            if (userId == null)
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            PerfilClienteDTO? perfil = await _clienteServices.GetPerfilAsync(userId.Value);
            return Ok(perfil);
        }

        [HttpPut("UpdatePerfilCliente")]
        public async Task<IActionResult> UpdatePerfilCliente([FromBody] UpdatePerfilClienteDTO dto)
        {
            Guid? userId = _userSessionServices.GetCurrentUserId();
            if (userId == null)
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            bool success = await _clienteServices.UpdatePerfilAsync(userId.Value, dto);

            if (success)
            {
                HttpContext.Session.SetString("FullName", $"{dto.Nombre} {dto.Apellido}");
            }

            return Ok(success);
        }

        #endregion

        #region Mascotas

        [HttpGet("GetMascotasCliente")]
        public async Task<IActionResult> GetMascotasCliente()
        {
            Guid? userId = _userSessionServices.GetCurrentUserId();
            if (userId == null)
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            List<MascotaClienteDTO> mascotas = await _clienteServices.GetMascotasByUsuarioAsync(userId.Value);
            return Ok(mascotas);
        }

        [HttpGet("GetMascotaClienteById")]
        public async Task<IActionResult> GetMascotaClienteById(int id)
        {
            Guid? userId = _userSessionServices.GetCurrentUserId();
            if (userId == null)
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            MascotaClienteDTO? mascota = await _clienteServices.GetMascotaByIdAsync(userId.Value, id);
            return Ok(mascota);
        }

        [HttpPost("AddMascotaCliente")]
        public async Task<IActionResult> AddMascotaCliente([FromForm] AddMascotasDTO add)
        {
            Guid? userId = _userSessionServices.GetCurrentUserId();
            if (userId == null)
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            bool success = await _clienteServices.AddMascotaAsync(userId.Value, add);
            return Ok(success);
        }

        [HttpPut("UpdateMascotaCliente")]
        public async Task<IActionResult> UpdateMascotaCliente([FromForm] UpdateMascotasDTO update)
        {
            Guid? userId = _userSessionServices.GetCurrentUserId();
            if (userId == null)
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            bool success = await _clienteServices.UpdateMascotaAsync(userId.Value, update);
            return Ok(success);
        }

        [HttpDelete("DeleteMascotaCliente")]
        public async Task<IActionResult> DeleteMascotaCliente(int id)
        {
            Guid? userId = _userSessionServices.GetCurrentUserId();
            if (userId == null)
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            bool success = await _clienteServices.DeleteMascotaAsync(userId.Value, id);
            return Ok(success);
        }

        #endregion
    }
}