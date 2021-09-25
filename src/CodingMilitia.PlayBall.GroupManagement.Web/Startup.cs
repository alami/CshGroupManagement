using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            
            //--using IOptions
            //services.Configure<SomeSubRootConfiguration>(_config.GetSection("SomeRoot"));
            
            //--injecting POCO without IOptions
            // var someRootConfiguration = new SomeRootConfiguration();
            // _config.GetSection("SomeRoot").Bind(someRootConfiguration);
            // services.AddSingleton(someRootConfiguration);

            //--injecting POCO but prettier
            services.ConfigurePOCO<SomeRootConfiguration>(_config.GetSection("SomeRoot")); 
            
            services.ConfigurePOCO<DemoSecretsConfiguration>(_config.GetSection("DemoSecrets")); 
            
            //--если использовать DI контейнер поумолчанию, раскомментировать
            //services.AddBusiness();
            
            //--add autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<AutofacModule>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);

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
        }
    }
}
