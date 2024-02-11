using FluentValidation;
using MetricsConfiguration.Domain.Model.Rule;
using MetricsConfiguration.Domain.Model.Rule.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsConfiguration.Domain
{
    public static class RegisterService
    {
        public static void ConfigureDomain(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateRuleRequest>, CreateRuleValidator>();
        }
    }
}