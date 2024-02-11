FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["3 - src/MetricsConfiguration.Api/MetricsConfiguration.Api.csproj", "3 - src/MetricsConfiguration.Api/"]
COPY ["3 - src/MetricsConfiguration.Domain/MetricsConfiguration.Domain.csproj", "3 - src/MetricsConfiguration.Domain/"]
COPY ["3 - src/MetricsConfiguration.Infrastructure/MetricsConfiguration.Infrastructure.csproj", "3 - src/MetricsConfiguration.Infrastructure/"]
COPY ["3 - src/MetricsConfiguration.Service/MetricsConfiguration.Service.csproj", "3 - src/MetricsConfiguration.Service/"]
RUN dotnet restore "3 - src/MetricsConfiguration.Api/MetricsConfiguration.Api.csproj"
COPY . .
WORKDIR "/src/3 - src/MetricsConfiguration.Api"
RUN dotnet build "MetricsConfiguration.Api.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "MetricsConfiguration.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetricsConfiguration.Api.dll"]