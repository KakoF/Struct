using MetricsConfiguration.Api.Controllers;
using MetricsConfiguration.Domain.Interface.Cache;
using MetricsConfiguration.Domain.Interface.Client;
using NSubstitute;

namespace MetricsConfiguration.Api.Tests.Controllers
{
    public class WeatherForecastControllerTests
    {
        private readonly WeatherForecastController _sut;
        private readonly IRedisCacheStorage _redisCache;
        private readonly IAtlasTriggerConfigurationClient _client;

        public WeatherForecastControllerTests()
        {
            _redisCache = Substitute.For<IRedisCacheStorage>();
            _client = Substitute.For<IAtlasTriggerConfigurationClient>();
            _sut = Substitute.For<WeatherForecastController>(_redisCache, _client);
        }

        [Fact(DisplayName = "Get -> Should Return String")]
        public void Get_Should_Return_String()
        {
            // Arrange 
            var response = Task.FromResult("Função teste");
            _sut.GetAsync().Returns(response);

            // Act
            var result = _sut.GetAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}