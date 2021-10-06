using System;
using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequiredMvcComponents(this IServiceCollection services)
        {
            var mvcBilder = services.AddMvcCore(option =>
            {
                option.EnableEndpointRouting = false;  //[ <---- МОЯ РЕДАКЦИЯ: иначе не запускается]
            });
            // mvcBilder.AddJsonFormatters();
            return services;
        }
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            // services.AddSingleton<IGroupsService, InMemoryGroupsService>();
            services.AddScoped<IGroupsService, GroupService>();
            // more business services ....
            return services;
        }
        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
 
            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }
    }
}