using Microsoft.EntityFrameworkCore;
using MVC.Data.DataContext;
using MVC.Data.Seed;
using MVC.Domain.servicios.Clientes;
using MVC.Domain.servicios.Clientes.interfaces;
using MVC.Domain.servicios.Mascotas;
using MVC.Domain.servicios.Mascotas.interfaces;
using MVC.Domain.servicios.seguridad;
using MVC.Domain.servicios.seguridad.interfaces;
using Veterinaria.Handlers;

namespace Veterinaria
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //builder.Services.AddDbContext
            builder.Services.AddDbContext<VeterinariaBdContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringVeterinaria_SQL_SERVER"));
            });


            // Inyección de dependencias
            builder.Services.AddScoped<IMascotasServicescs, MascotasServices>();
            builder.Services.AddScoped<IEspecieServices, EspecieServices>();
            builder.Services.AddScoped<IRazaServices, RazaServices>();
            builder.Services.AddScoped<IPropietariosServices, PropietariosServices>();
            builder.Services.AddScoped<IFileServices, FileServices>();
            builder.Services.AddScoped<IHostingEviromentServices, HostingEviromentHandler>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IClienteServices, ClienteServices>();

            // Seed de datos iniciales
            builder.Services.AddScoped<SeedDb>();

            #region Multipart & Upload configuration
            builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104_857_600; // 100 MB
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 104_857_600; // 100 MB
            });
            #endregion

            var app = builder.Build();

            // Ejecutar el seed al iniciar
            using (var scope = app.Services.CreateScope())
            {
                var seedDb = scope.ServiceProvider.GetRequiredService<SeedDb>();
                await seedDb.SeedAsync();
            }

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

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
