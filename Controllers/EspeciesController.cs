using Microsoft.AspNetCore.Mvc;

namespace Veterinaria.Controllers
{
    public class EspeciesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
