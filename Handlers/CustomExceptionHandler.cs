using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO;
using Microsoft.AspNetCore.Mvc.Filters;
using MVC.Common.Execptions;

namespace Veterinaria.Handlers
{
    public class CustomExceptionHandler: ExceptionFilterAttribute
    {

        //aqui estamos sobreescribiendo una propiedad ya existente llamada 
        //OnException con el override, entonces todo error que suceda entra qui y no usamos al try cath
        public override void OnException(ExceptionContext context)
        {
            HTTPResponseException responseException = new HTTPResponseException();

             ResponseDto response = new ResponseDto();
            if (context.Exception is BusinessException)
            {
                responseException.Status = StatusCodes.Status400BadRequest;
                response.Message = context.Exception.Message;
                context.ExceptionHandled = true;
                //Log.Error(context.Exception, response.Message);
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                responseException.Status = StatusCodes.Status401Unauthorized;
                //response.Message = GeneralMessages.Error401;
                response.Message = "Usuario no autenticado";
                context.ExceptionHandled = true;
                // Log.Error(GeneralMessages.Error401);
            }
            else
            {
                responseException.Status = StatusCodes.Status500InternalServerError;
                //response.Result = JsonConvert.SerializeObject(context.Exception);
                //response.Message = GeneralMessages.Error500;
                response.Message = "Ha ocurrido un error interno, por favor vuelva a intentarlo.  123456";
                context.ExceptionHandled = true;

                //Add Logs
                //Log.Fatal(context.Exception, GeneralMessages.Error500);

            }

            context.Result = new ObjectResult(responseException.Value)
            {
                StatusCode = responseException.Status,
                Value = response
            };

            //if (responseException.Status == StatusCodes.Status500InternalServerError)
            //{
            //context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = GeneralMessages.Error500;
            //context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Ha ocurrido un error interno, por favor vuelva a intentarlo.";
            //}
        }
    }
}
