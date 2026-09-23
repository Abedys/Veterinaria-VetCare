using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO;
using MVC.Data.DTO.User;
using MVC.Domain.servicios.seguridad.interfaces;
using Veterinaria.Handlers;

namespace Veterinaria.Controllers
{
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class UserController : Controller
    {

        private readonly IUserServices _userServices;


        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
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
            var user = _userServices.Login(login);

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("FullName", $"{user.Nombre} {user.Apellido}");
            HttpContext.Session.SetString("UserRol", user.Rol);
            HttpContext.Session.SetString("DebeCambiarPassword", user.DebeCambiarPassword ? "true" : "false");

            return Ok(new ResponseDto { Success = true, Result = user });
        }


        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser (AddUserDTO register)
        {
            bool result = await _userServices.RegistreUser(register);
            return Ok(new ResponseDto { Success = result });
        }

        [HttpPost]
        [Route("ChangePassword")]
        [TypeFilter(typeof(CustomExceptionHandler))]
        [UserRol]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Ok(new ResponseDto { Success = false, Message = "No autorizado. Debes iniciar sesión." });
            }

            bool result = await _userServices.ChangePasswordAsync(Guid.Parse(userId), dto);

            if (result)
            {
                HttpContext.Session.SetString("DebeCambiarPassword", "false");
            }

            return Ok(new ResponseDto { Success = result });
        }








    }
}
