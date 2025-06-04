using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Net;
using System.Text.Json;
using Taxi_Booking.Exceptions;
using Taxi_Booking.Models;
using Taxi_Booking.Models.Responses;

namespace Taxi_Booking.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                context.Response.ContentType = "application/json";
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                string errorMessage = exception.Message;
                List<string> errorMessages=null;

                switch (exception)
                {
                    case UnauthorizedAccessException:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;
                    case BadRequestException:
                    case ArgumentNullException:
                    case ArgumentException:
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException:
                        break;
                    case InvalidCredentialException:
                        break;
                    case ValidationException ve:
                        //pattern matching syntax
                        //Is this exception an instance of ValidationException ?
                        //If it is, it casts the exception to a variable named ve
                        if (ve.Errors!=null && ve.Errors.Any()) errorMessages = ve.Errors;
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    default:
                        break;
                }
                context.Response.StatusCode = (int)statusCode;
                ErrorResponse response = new ErrorResponse(errorMessage, (int)statusCode, _env.IsDevelopment() ? exception.StackTrace : null,errorMessages!=null?errorMessages:null);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }

        }
    }
}
