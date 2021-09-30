﻿using System;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares;
using CodingMilitia.PlayBall.GroupManagement.Web.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
//            var value =  _config.GetValue<int>("someRoot:someSubRoot:SomeKey");
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;  //[ <---- МОЯ РЕДАКЦИЯ: иначе не запускается]
                option.Filters.Add<DemoActionFilter>();
            });
            services.AddTransient<RequestTimingFactoryMiddleware>();
            services.AddTransient<DemoExceptionFilter>();
            
            //--если использовать DI контейнер поумолчанию, раскомментировать
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

            app.Map("/ping", builder =>
            {
                builder.UseMiddleware<RequestTimingFactoryMiddleware>();
                builder.Run(async (context) => { await context.Response.WriteAsync("pong"); });
            });
            
            app.MapWhen(
                context => context.Request.Headers.ContainsKey("ping"), 
                builder =>
                    {
                        builder.UseMiddleware<RequestTimingAdHocMiddleware>();
                        builder.Run(async (context) => { await context.Response.WriteAsync("pong from header"); });
                    });

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
