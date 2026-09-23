using System.Diagnostics;
using Veterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Veterinaria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string? rol = HttpContext.Session.GetString("UserRol");

            if (string.IsNullOrEmpty(rol))
            {
                return View();
            }

            if (rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Mascotas", "Clientes");
            }

            return RedirectToAction("Index", "Mascotas");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
