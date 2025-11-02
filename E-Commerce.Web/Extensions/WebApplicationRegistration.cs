using Domain.Contracts;
using E_Commerce.Web.CustomeMiddleWares;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;

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
            app.UseSwaggerUI(options =>
            {
                options.ConfigObject = new ConfigObject()
                {
                    DisplayRequestDuration = true,

                };

                options.DocumentTitle = "RestFull Api Project";

                options.JsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                options.DocExpansion(DocExpansion.None);

                options.EnableFilter();

                options.EnablePersistAuthorization();
            });
            return app;
        }
    }
}
