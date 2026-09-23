using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO.Mascotas;
using MVC.Data.DTO.User;
using MVC.Domain.servicios.Clientes.interfaces;
using MVC.Domain.servicios.Mascotas.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    [ClienteRol]
    public class ClientesController : Controller
    {
        private readonly IClienteServices _clienteServices;
        private readonly IEspecieServices _especieServices;
        private readonly IRazaServices _razaServices;

        public ClientesController(IClienteServices clienteServices, IEspecieServices especieServices, IRazaServices razaServices)
        {
            _clienteServices = clienteServices;
            _especieServices = especieServices;
            _razaServices = razaServices;
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
            Guid userId = GetUserId();
            PerfilClienteDTO? perfil = await _clienteServices.GetPerfilAsync(userId);
            return Ok(perfil);
        }

        [HttpPut("UpdatePerfilCliente")]
        public async Task<IActionResult> UpdatePerfilCliente([FromBody] UpdatePerfilClienteDTO dto)
        {
            Guid userId = GetUserId();
            bool success = await _clienteServices.UpdatePerfilAsync(userId, dto);

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
            Guid userId = GetUserId();
            List<MascotaClienteDTO> mascotas = await _clienteServices.GetMascotasByUsuarioAsync(userId);
            return Ok(mascotas);
        }

        [HttpGet("GetMascotaClienteById")]
        public async Task<IActionResult> GetMascotaClienteById(int id)
        {
            Guid userId = GetUserId();
            MascotaClienteDTO? mascota = await _clienteServices.GetMascotaByIdAsync(userId, id);
            return Ok(mascota);
        }

        [HttpPost("AddMascotaCliente")]
        public async Task<IActionResult> AddMascotaCliente([FromForm] AddMascotasDTO add)
        {
            Guid userId = GetUserId();
            bool success = await _clienteServices.AddMascotaAsync(userId, add);
            return Ok(success);
        }

        [HttpPut("UpdateMascotaCliente")]
        public async Task<IActionResult> UpdateMascotaCliente([FromForm] UpdateMascotasDTO update)
        {
            Guid userId = GetUserId();
            bool success = await _clienteServices.UpdateMascotaAsync(userId, update);
            return Ok(success);
        }

        [HttpDelete("DeleteMascotaCliente")]
        public async Task<IActionResult> DeleteMascotaCliente(int id)
        {
            Guid userId = GetUserId();
            bool success = await _clienteServices.DeleteMascotaAsync(userId, id);
            return Ok(success);
        }

        #endregion

        #region Combos (Especies / Razas)

        [HttpGet("GetEspeciesCliente")]
        public async Task<IActionResult> GetEspeciesCliente()
        {
            var especies = await _especieServices.GetAllEspeciesAsync();
            return Ok(especies);
        }

        [HttpGet("GetRazasByEspecieCliente")]
        public async Task<IActionResult> GetRazasByEspecieCliente(int idEspecie)
        {
            var razas = await _razaServices.GetRazasByEspecieAsync(idEspecie);
            return Ok(razas);
        }

        #endregion

        private Guid GetUserId()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        }
    }
}