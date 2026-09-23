using MVC.Domain.servicios.Mascotas.interfaces;

namespace Veterinaria.Handlers
{
    public class HostingEviromentHandler: IHostingEviromentServices
    {

        private readonly IWebHostEnvironment _webHostEnvironment;


        public HostingEviromentHandler(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        //son propiedades de lectura
        public string WebRootPath => _webHostEnvironment.WebRootPath;

        public string ContentRootPath => _webHostEnvironment.ContentRootPath;


    }
}
