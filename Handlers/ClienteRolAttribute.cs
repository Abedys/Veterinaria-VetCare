using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVC.Common.Enums;
using MVC.Data.DTO;

namespace Veterinaria.Handlers
{
    public class ClienteRolAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? rol = context.HttpContext.Session.GetString("UserRol");

            bool isCliente = !string.IsNullOrEmpty(rol) &&
                             rol.Equals(Enumeraciones.RolUser.Cliente.ToString(), StringComparison.OrdinalIgnoreCase);

            if (!isCliente)
            {
                if (IsAjaxRequest(context))
                {
                    context.Result = new UnauthorizedObjectResult(new ResponseDto
                    {
                        Success = false,
                        Message = "No autorizado. Debes iniciar sesión como cliente."
                    });
                }
                else
                {
                    context.Result = new RedirectToActionResult("Index", "User", null);
                }
                return;
            }

            base.OnActionExecuting(context);
        }

        private static bool IsAjaxRequest(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}