using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVC.Common.Enums;
using MVC.Data.DTO;

namespace Veterinaria.Handlers
{
    public class AdminRolAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? rol = context.HttpContext.Session.GetString("UserRol");

            bool isAdmin = !string.IsNullOrEmpty(rol) &&
                           (rol.Equals(Enumeraciones.RolUser.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase) ||
                            rol.Equals(Enumeraciones.RolUser.Admin.ToString(), StringComparison.OrdinalIgnoreCase));

            if (!isAdmin)
            {
                if (IsAjaxRequest(context))
                {
                    context.Result = new UnauthorizedObjectResult(new ResponseDto
                    {
                        Success = false,
                        Message = "No autorizado. Debes iniciar sesión con permisos de administrador."
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