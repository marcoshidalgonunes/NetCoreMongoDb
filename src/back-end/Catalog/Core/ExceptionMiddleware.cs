using System;
using System.Net;
using System.Text.Json;
using System.Text;
using Catalog.Domain.Models;

namespace Catalog.Core;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception ocurred:{NewLine}{ex}", Environment.NewLine, ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var isValidationException = exception is FluentValidation.ValidationException;
        var statusCode = isValidationException
            ? (int)HttpStatusCode.UnprocessableEntity
            : (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = isValidationException
            ? new ErrorResponse()
            {
                Message = "One or more fields are invalid",
                Details = ((FluentValidation.ValidationException)exception).Errors.Select(err => new ValidationError()
                {
                    Field = err.PropertyName,
                    Message = err.ErrorMessage,
                    AttemptedValue = err.AttemptedValue
                }).ToList()
                    
            }
            : new ErrorResponse() { Message = "Internal Server Error. Please try again later." };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}
