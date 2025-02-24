using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ExpenseTracker.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //Pass request to the next middleware or endpoint
                await _next(context);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Unhandled exception occurred while processing the request.");

                //Handle the exception and return a proper error response
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            
            var problemDetails = new ProblemDetails
            {
                Title = "An unhandled exception occurred while processing the request.",
                Detail = ex.Message
                
            };
            if (ex is ArgumentException)
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = ex.Message; 
            }
            else if (ex is UnauthorizedAccessException)
            {
                problemDetails.Status = StatusCodes.Status401Unauthorized;
            }
            else if (ex is InvalidOperationException)
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
            }
            else
            {
                problemDetails.Status = StatusCodes.Status500InternalServerError;
            }


            context.Response.StatusCode = (int)(problemDetails.Status ?? 500);
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(problemDetails);
        }

    }
}
