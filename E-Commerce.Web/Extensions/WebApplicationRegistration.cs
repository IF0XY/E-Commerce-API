using Domain.Contracts;
using E_Commerce.Web.CustomeMiddleWares;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task SeedData(this WebApplication app)
        {
            #region DataSeeding
            #region Dependancy Injiction
            var Scope = app.Services.CreateScope();

            var seed = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            #endregion

            await seed.DataSeedAsync();
            await seed.IdentityDataSeedAsync();
            #endregion
        }

        public static IApplicationBuilder UseCustomeExceptionMiddleWate(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddlerWare>();
            return app;
        }

        public static IApplicationBuilder UseSwaggerMiddleWares(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
