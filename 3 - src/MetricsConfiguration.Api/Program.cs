using FluentValidation.AspNetCore;
using MetricsConfiguration.Api.Filter;
using MetricsConfiguration.Api.Middleware;
using MetricsConfiguration.Domain;
using MetricsConfiguration.Domain.Interfaces.Notifications;
using MetricsConfiguration.Domain.Notifications;
using MetricsConfiguration.Infrastructure;
using MetricsConfiguration.Infrastructure.Environment;
using MetricsConfiguration.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

NLog.Extensions.Logging.ConfigSettingLayoutRenderer.DefaultConfiguration = builder.Configuration;
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
var logger =
    NLogBuilder.ConfigureNLog(AspNetEnvironment.IsProductionOrStaging() ?
    "nlog.config" :
    $"nlog.{AspNetEnvironment.GetEnvironment()}.config")
        .GetCurrentClassLogger();

#pragma warning disable CS0618 // O tipo ou membro é obsoleto
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ValidationFilter));
    options.Filters.Add(typeof(NotificationFilter));
}).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssembly(typeof(Program).Assembly));
#pragma warning restore CS0618 // O tipo ou membro é obsoleto

builder.Services.Configure<CacheConfigs>(builder.Configuration.GetSection("Cache"));
builder.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<CacheConfigs>>().Value);
builder.Services.Configure<AuthorizationApps>(builder.Configuration.GetSection("AthorizationApps"));
builder.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<AuthorizationApps>>().Value);

builder.Services.ConfigureDomain();
builder.Services.ConfigureInfraStructure(builder.Configuration);
builder.Services.ConfigureService();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<INotificationHandler<Notification>, NotificationHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.CustomSchemaIds(type => type.ToString());
    c.AddSecurityDefinition("App-Authorization-Id", new OpenApiSecurityScheme
    {
        Name = "App-Authorization-Id",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "Id da aplicação"
    });
    c.OperationFilter<AuthorizationAppsHeaders>();
});

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware(typeof(ErrorHandlingMiddleware));
app.UseMiddleware<MiddlewareAppsAuthorization>();
app.UseAuthorization();

app.MapControllers();

app.Run();
