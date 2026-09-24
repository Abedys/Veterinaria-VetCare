using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO;
using MVC.Data.DTO.User;
using MVC.Data.DTO.UserSession;
using MVC.Domain.servicios.seguridad.interfaces;
using System;
using System.Threading.Tasks;
using Veterinaria.Filters;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IUserSessionServices _userSessionServices;

        public UserController(IUserServices userServices, IUserSessionServices userSessionServices)
        {
            _userServices = userServices;
            _userSessionServices = userSessionServices;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginDTO login)
        {
            LoginUserDTO user = _userServices.Login(login);
            _userSessionServices.CreateSession(user);

            return Ok(new ResponseDto { Success = true, Result = user });
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            _userSessionServices.ClearSession();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser(AddUserDTO register)
        {
            bool result = await _userServices.RegistreUser(register);
            return Ok(new ResponseDto { Success = result });
        }

        [HttpPost]
        [Route("ChangePassword")]
        [UserRol]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            Guid? userId = _userSessionServices.GetCurrentUserId();
            if (userId == null)
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            bool result = await _userServices.ChangePasswordAsync(userId.Value, dto);

            if (result)
            {
                _userSessionServices.MarcarPasswordCambiada();
            }

            return Ok(new ResponseDto { Success = result });
        }
    }
}