using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task4_Authorisation.Data;
using Task4_Authorisation.Data.Interfaces;
using Task4_Authorisation.Data.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            services.AddDbContext<AppDBContent>(options => options.UseSqlServer(connection));
            services.AddTransient<IUsers, UsersRepository>();
            services.AddMvc();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login"));
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
