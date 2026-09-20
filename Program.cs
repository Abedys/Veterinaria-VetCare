using Microsoft.EntityFrameworkCore;
using MVC.Data.DataContext;
using MVC.Domain.servicios;
using MVC.Domain.servicios.interfaces;
using Veterinaria.Handlers;

namespace Veterinaria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddDbContext
            builder.Services.AddDbContext<VeterinariaBdContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringVeterinaria_SQL_SERVER"));
            });


            builder.Services.AddScoped<IMascotasServicescs, MascotasServices>();
            builder.Services.AddScoped<IHostingEviromentServices, HostingEviromentHandler>


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
