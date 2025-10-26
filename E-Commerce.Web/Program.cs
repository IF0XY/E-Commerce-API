
using Domain.Contracts;
using E_Commerce.Web.CustomeMiddleWares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistence;
using Presistence.Data.Contexts;
using Presistence.Repositories;
using Service;
using Service.Abstraction;
using Service.MappingProfiles;
using Shared.ErrorModels;
using System.Reflection;

namespace E_Commerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container
            // Add services to the container.

            builder.Services.AddControllers(); // We Dont Have Views
                                               // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServicess();
            // DBContext
            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddApplicationServices();
            // Validation Api Response 
            builder.Services.AddWebApplicationServices();
            builder.Services.AddJWTServices(builder.Configuration);
            #endregion

            var app = builder.Build();


            app.SeedData();

            #region Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.

            ///app.Use(async (RequestContext, NextMiddleWare) =>
            ///{
            ///    Console.WriteLine("Request Under Processing");
            ///    await NextMiddleWare.Invoke();
            ///    Console.WriteLine("Waiting Response");
            ///    Console.WriteLine(RequestContext.Response.Body);
            ///});

            app.UseCustomeExceptionMiddleWate();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
