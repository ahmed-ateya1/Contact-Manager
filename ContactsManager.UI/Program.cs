using ContactAppManager.Middleware;

namespace ContactAppManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandlingMiddleware();
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHsts();
            app.UseHttpsRedirection();
            Rotativa.AspNetCore.RotativaConfiguration
                .Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Persons}/{action=Index}/{id?}"
                );
            });

            app.Run();
        }
    }
}
