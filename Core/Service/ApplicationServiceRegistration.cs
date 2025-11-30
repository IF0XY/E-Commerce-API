using Microsoft.Extensions.DependencyInjection;
using Service.Abstraction;
using Service.MappingProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddAutoMapper(config => config.AddProfile(new ProductProfile()), typeof(Service.AssemblyReference).Assembly);

            Services.AddScoped<IServiceManager, ServiceManager>();
            //Services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();

            //Services.AddKeyedScoped<IServiceManager, ServiceManager>("Lazy");
            //Services.AddKeyedScoped<IServiceManager, ServiceManagerWithFactoryDelegate>("FactoryDelegate");

            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<Func<IProductService>>(prvider =>
            {
                return () => prvider.GetRequiredService<IProductService>();
            });

            Services.AddScoped<IBasketService, BasketService>();
            Services.AddScoped<Func<IBasketService>>(prvider =>
            {
                return () => prvider.GetRequiredService<IBasketService>();
            });

            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<Func<IAuthenticationService>>(prvider =>
            {
                return () => prvider.GetRequiredService<IAuthenticationService>();
            });

            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<Func<IOrderService>>(prvider =>
            {
                return () => prvider.GetRequiredService<IOrderService>();
            });


            Services.AddScoped<ICacheService, CacheService>();
            return Services;
        }
    }
}
