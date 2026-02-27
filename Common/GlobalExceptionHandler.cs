using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Warehouse.Common
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
            => _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";

                var errors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Success = false,
                    Message = "Validation failed.",
                    Errors = errors
                }, cancellationToken);

                return true;
            }

            _logger.LogError(exception, "Unhandled exception occurred.");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(new
            {
                Success = false,
                Message = "An unexpected error occurred. Please try again later."
            }, cancellationToken);

            return true;
        }
    }
}
