using MetricsConfiguration.Domain.Interface.Services;
using MetricsConfiguration.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsConfiguration.Service
{
    public static class RegisterService
    {
        public static void ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<IAdviceService, AdviceService>();
            services.AddScoped<IChuckService, ChuckService>();
            services.AddScoped<IViabilidadeAnaliticaService, ViabilidadeAnaliticaService>();
        }
    }
}
