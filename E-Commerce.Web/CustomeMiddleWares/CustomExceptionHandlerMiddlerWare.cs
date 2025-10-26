using Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.ErrorModels;
using System.Net;
using System.Text.Json;

namespace E_Commerce.Web.CustomeMiddleWares
{
    public class CustomExceptionHandlerMiddlerWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddlerWare> _logger;

        public CustomExceptionHandlerMiddlerWare(RequestDelegate next, ILogger<CustomExceptionHandlerMiddlerWare> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                // Request
                await _next.Invoke(httpcontext);

                //Response
                await HandelNotFoundPointAsync(httpcontext);
            }
            catch (Exception ex)
            {
                await HandelExceptionAsync(httpcontext, ex);
            }
        }

        private async Task HandelExceptionAsync(HttpContext httpcontext, Exception ex)
        {
            _logger.LogError(ex, "Somthing Went Wrong");

            // Set Status Code For Response
            //httpcontext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            httpcontext.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnAutherizedException => StatusCodes.Status401Unauthorized,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };

            // Set Content Type for Response
            //httpcontext.Response.ContentType = "application/json"; // 01

            // Response Object
            var response = new ErrorToReturn()
            {
                StatusCode = httpcontext.Response.StatusCode,
                ErrorMassage = ex.Message,
                Errors = ex switch
                {
                    BadRequestException badRequestException => badRequestException.Errors,
                    _ => []
                }
            };

            // Transfer Object to Json
            //var responseJson = JsonSerializer.Serialize(response); // 02

            //await httpcontext.Response.WriteAsync(responseJson); // 03

            await httpcontext.Response.WriteAsJsonAsync(response); // 01 : 03
        }

        private static async Task HandelNotFoundPointAsync(HttpContext httpcontext)
        {
            if (httpcontext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMassage = $"End Point ${httpcontext.Request.Path} is Not Found"
                };
                await httpcontext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
