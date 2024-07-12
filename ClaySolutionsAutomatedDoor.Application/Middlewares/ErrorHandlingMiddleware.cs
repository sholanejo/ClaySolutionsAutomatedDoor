using ClaySolutionsAutomatedDoor.Application.Common.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ClaySolutionsAutomatedDoor.Application.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                if (ex is InvalidOperationException invalidOperationException)
                {
                    _logger.LogInformation(invalidOperationException, invalidOperationException.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    var responseObject = new BaseResponse<object>
                    {
                        Status = false,
                        Message = "You do not have access to this resource",
                        StatusCode = StatusCodes.Status403Forbidden,
                    };

                    var json = JsonSerializer.Serialize(responseObject);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
                else if (ex is ValidationException validationException)
                {
                    _logger.LogInformation(validationException, validationException.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var responseObject = new BaseResponse<object>
                    {
                        Status = false,
                        Message = ex.Message,
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                    var json = JsonSerializer.Serialize(responseObject);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
                else
                {

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var responseObject = new BaseResponse<object>
                    {
                        Status = false,
                        Message = "An error occured while processing",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };

                    var json = JsonSerializer.Serialize(responseObject);

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
                _logger.LogError(ex, "An unhandled exception occurred.");

            }
        }
        private string GetValidationErrors(ValidationException ex)
        {
            return string.Join(",", ex.Errors.SelectMany(e => e.ErrorMessage));
        }
    }
}
