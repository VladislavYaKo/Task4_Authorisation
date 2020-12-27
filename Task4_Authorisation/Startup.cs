using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Task4_Authorisation.Data;
using Task4_Authorisation.Data.Interfaces;
using Task4_Authorisation.Data.Middlewares;
using Task4_Authorisation.Data.Repository;
using Task4_Authorisation.Data.Services;

namespace Task4_Authorisation
{
    public class Startup
    {
        private IConfigurationRoot configuration;

        public Startup(IWebHostEnvironment hostEnv)
        {
            configuration = new ConfigurationBuilder().SetBasePath(hostEnv.ContentRootPath).AddJsonFile("dbsettings.json").Build();            
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connection));
            services.AddScoped<IUsers, UsersRepository>();
            services.AddSingleton<TrackStatusesChangeService, TrackStatusesChangeService>();
            services.AddMvc();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login"));
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
                options.Cookie.IsEssential = true
            );   
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseMiddleware<CheckStatusMiddleware>();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");
                endpoints.MapControllerRoute(
                    name: "registration",
                    pattern: "Registration",
                    defaults: new { controller = "Account", action = "Register" });
            });            

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hi Mark!");
            });            
        }
    }
}
