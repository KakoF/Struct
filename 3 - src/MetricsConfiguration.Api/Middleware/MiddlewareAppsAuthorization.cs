using MetricsConfiguration.Domain.Exceptions;
using MetricsConfiguration.Infrastructure.Environment;
using System.Security.Claims;

namespace MetricsConfiguration.Api.Middleware
{
    public class MiddlewareAppsAuthorization
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareAppsAuthorization> _logger;
        private readonly IHttpContextAccessor _accessor;

        public MiddlewareAppsAuthorization(RequestDelegate next, ILogger<MiddlewareAppsAuthorization> logger, IHttpContextAccessor accessor)
        {
            _next = next;
            _logger = logger;
            _accessor = accessor;
        }

        public async Task InvokeAsync(HttpContext httpContext, AuthorizationApps authorization)
        {
            if (!_accessor.HttpContext.Request.Headers["App-Authorization-Id"].Any())
                throw new DomainException("Unauthorized", 401);

            var appId = _accessor.HttpContext.Request.Headers["App-Authorization-Id"];

            var app = authorization.Apps.FirstOrDefault(x => x.Id == appId);
            if (app != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, app.Id),
                        new Claim(ClaimTypes.Name, app.Name),
                    };
                var appIdentity = new ClaimsIdentity(claims);
                httpContext.User.AddIdentity(appIdentity);
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
                await _next(httpContext);
            }
            else
            {
                _logger.LogWarning("Aplicação não autorizada: {0}", appId);
                throw new DomainException("Unauthorized", 401);
            }

        }
    }
}
