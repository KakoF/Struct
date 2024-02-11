using MetricsConfiguration.Domain.Exceptions;
using MetricsConfiguration.Domain.Model.Error;
using Newtonsoft.Json;
using System.Net;

namespace MetricsConfiguration.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int code;
            ErrorResponse error;

            if (exception is DomainException)
            {
                var ex = (DomainException)exception;
                code = ex.StatusCode;
                error = new ErrorResponse(code, exception.Message);
            }
            else
            {
                code = (int)HttpStatusCode.InternalServerError;
                error = new ErrorResponse(code);
            }
            if (code >= 500)
                _logger.LogError(exception, exception.Message);
            else if (code >= 400)
                _logger.LogWarning(exception, exception.Message);
            else
                _logger.LogDebug(exception, exception.Message);
            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}