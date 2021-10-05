using System;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Data;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares;
using CodingMilitia.PlayBall.GroupManagement.Web.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.PlayBall.GroupManagement.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;  //[ <---- МОЯ РЕДАКЦИЯ: иначе не запускается]
            });

            services.AddDbContext<GroupManagementDbContext>(option =>
            {
                option.UseNpgsql("Server=localhost;Port=54331;Username=postgres;Password=postgres;Database=GroupManagement");
                // option.UseNpgsql(_config.GetConnectionString("GroupManagementDbContext"));
            });
            services.AddTransient<DemoExceptionFilter>();
            
            services.AddBusiness(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("X-Powered-By", "ASP.net Code: From 0 to overkill");
                    return Task.CompletedTask;
                });
                await next.Invoke();    
            });
            app.UseMvc();
            
            app.Run(async (context) => { await context.Response.WriteAsync("No middlerware could handle the request"); });
        }
    }
}
