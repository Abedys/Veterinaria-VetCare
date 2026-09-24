using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVC.Data.DTO;

namespace Veterinaria.Filters
{
    public class UserRolAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? userId = context.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                if (IsAjaxRequest(context))
                {
                    context.Result = new UnauthorizedObjectResult(new ResponseDto
                    {
                        Success = false,
                        Message = "No autorizado. Debes iniciar sesión."
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