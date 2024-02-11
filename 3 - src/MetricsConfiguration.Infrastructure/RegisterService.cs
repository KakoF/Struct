using MetricsConfiguration.Domain.Interface.Cache;
using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interface.Repositories;
using MetricsConfiguration.Infrastructure.Cache;
using MetricsConfiguration.Infrastructure.Client;
using MetricsConfiguration.Infrastructure.DataConnector;
using MetricsConfiguration.Infrastructure.Interfaces.DataConnector;
using MetricsConfiguration.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsConfiguration.Infrastructure
{
    public static class RegisterService
    {
        public static void ConfigureInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<IRedisCacheStorage, RedisCacheStorage>();
            services.AddScoped<IMemoryCacheStorage, MemoryCacheStorage>();
            services.AddScoped<IDbSqlServerConnector>(db => new SqlServerConnector(configuration.GetConnectionString("ViabilidadeSQLServer")));
            services.AddScoped<IDbPostgreConnector>(db => new PostgreConnector(configuration.GetConnectionString("ViabilidadePostgreSQL")));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Cache:RedisCache:Url"];
            });
            services.AddScoped<IAtlasTriggerConfigurationClient, AtlasTriggerConfigurationClient>();
            services.AddHttpClient("AtlasTriggerConfigurationClient", config =>
            {
                config.BaseAddress = new Uri(configuration["Clients:AtlasTriggerConfiguration:BasePath"]);
            });

            services.AddScoped<IAtlasViabilidadeClient, AtlasViabilidadeClient>();
            services.AddHttpClient("AtlasViabilidadeClient", config =>
            {
                config.BaseAddress = new Uri(configuration["Clients:AtlasViabilidade:BasePath"]);
            });

            services.AddScoped<IAdviceClient, AdviceClient>();
            services.AddHttpClient("AdviceClient", config =>
            {
                config.BaseAddress = new Uri(configuration["Clients:Advice:BasePath"]);
            });

            services.AddScoped<IChuckClient, ChuckClient>();
            services.AddHttpClient("ChuckClient", config =>
            {
                config.BaseAddress = new Uri(configuration["Clients:Chuck:BasePath"]);
            });

            services.AddScoped<IOriginRepository, OriginRepository>();

        }
    }
}
