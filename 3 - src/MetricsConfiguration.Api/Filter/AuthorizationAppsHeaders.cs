using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MetricsConfiguration.Api.Filter
{
    public class AuthorizationAppsHeaders : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var appId = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "App-Authorization-Id" } };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [appId] = new List<string>(),
            });
        }
    }
}