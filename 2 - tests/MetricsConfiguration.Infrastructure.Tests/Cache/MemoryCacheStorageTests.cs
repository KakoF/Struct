using MetricsConfiguration.Infrastructure.Cache;
using MetricsConfiguration.Infrastructure.Environment;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MetricsConfiguration.Infrastructure.Tests.Cache
{
    public class MemoryCacheStorageTests
    {
        private readonly MemoryCacheStorage _sut;
        private readonly IMemoryCache _memoryCache;
        private readonly CacheConfigs _cacheConfigs;
        private readonly ILogger<MemoryCacheStorage> _logger;

        public MemoryCacheStorageTests()
        {
            _cacheConfigs = new CacheConfigs() { MemoryCache = new Environment.MemoryCache() { Key = "Mock", Expriration_Minutes = 1 }, RedisCache = new RedisCache() { Expriration_Minutes = 1, Key = "Mock", Url = "Url" } };
            _memoryCache = Substitute.For<IMemoryCache>();
            _logger = Substitute.For<ILogger<MemoryCacheStorage>>();
            _sut = Substitute.For<MemoryCacheStorage>(_memoryCache, _cacheConfigs, _logger);
        }

        [Fact(DisplayName = "GetOrCreateAsync")]
        [Trait("GetOrCreateAsync", "Should Return Data Of Memory Cache")]
        public async Task GetOrCreateAsync_Should_Return_Data_Of_MemoryCache()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            var timeExpiration = DateTimeOffset.Now.AddSeconds(10);

            // Act
            var result = await _sut.GetOrCreateAsync(key, () => Task.FromResult(data), timeExpiration);

            // Assert
            Assert.Equal(2, _memoryCache.ReceivedCalls().Count());
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "GetOrCreateAsync")]
        [Trait("GetOrCreateAsync", "Should Execute Function And Store Result In Memory Cache")]
        public async Task GetOrCreateAsync_Should_Execute_Function_And_Store_Result_In_Memory_Cache()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            var timeExpiration = DateTimeOffset.Now.AddSeconds(10);

            // Act
            var result = await _sut.GetOrCreateAsync(key, () => Task.FromResult(data), timeExpiration);

            // Assert
            Assert.Equal(2, _memoryCache.ReceivedCalls().Count());
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Set")]
        [Trait("Set", "Should Store Data In Memory Cache With Expiration Date")]
        public void Set_ShouldStoreData_In_MemoryCache_WithExpirationDate()
        {

            // Arrange
            var key = "key";
            var data = new { data = "data" };
            var timeOffset = DateTimeOffset.Now.AddSeconds(10);

            // Act
            _sut.Set(key, data, timeOffset);

            // Assert
            Assert.Single(_memoryCache.ReceivedCalls());

        }

        [Fact(DisplayName = "Set")]
        [Trait("Set", "Should Store Data In Memory Cache Without Expiration Date")]
        public void Set_ShouldStoreData_In_MemoryCache_WithoutExpirationDate()
        {
            // Arrange
            var key = "key";
            var data = new { data = "data" };

            // Act
            _sut.Set(key, data);

            // Assert
            Assert.Single(_memoryCache.ReceivedCalls());
        }

        [Fact(DisplayName = "Set")]
        [Trait("Set", "Should Throws Exception And Loggin Error")]
        public void Set_ShouldThrowsException_And_LogginError()
        {
            // Arrange
            var key = "key";
            var data = new { data = "data" };
            var timeExpiration = DateTimeOffset.Now.AddSeconds(10);
            _memoryCache.When(x => x.CreateEntry(Arg.Any<object>())).Do(x => { throw new Exception(); });

            // Act
            _sut.Set(key, data, timeExpiration);

            // Assert
            Assert.Single(_logger.ReceivedCalls());
        }


        [Fact(DisplayName = "GetAsync")]
        [Trait("GetAsync", "Should Get Data In Memory Cache")]
        public async Task GetAsync_Should_Get_Data_In_Memory_CacheAsync()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            _memoryCache.Get(Arg.Any<object>()).Returns(_memoryCache.TryGetValue(key, out data));

            // Act
            var result = await _sut.GetAsync<object>(key);

            // Assert
            Assert.Single(_memoryCache.ReceivedCalls());
        }

        [Fact(DisplayName = "GetAsync")]
        [Trait("GetAsync", "Should Return Null Object And Loggin Error")]
        public async Task GetAsync_Should_Return_Null_Object_And_Loggin_Error()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            _memoryCache.When(x => x.Get(Arg.Any<object>())).Do(x => { throw new Exception(); });

            // Act
            var result = await _sut.GetAsync<object>(key);

            // Assert
            Assert.Single(_memoryCache.ReceivedCalls());
            Assert.Single(_logger.ReceivedCalls());
        }


        [Fact(DisplayName = "Remove")]
        [Trait("Remove", "Should Remove Index Of Memory Cache")]
        public void Remove_Should_Remove_Index_Of_Memory_Cache()
        {
            // Arrange
            var key = "key";

            // Act
            _sut.Remove(key);

            // Assert
            Assert.Single(_memoryCache.ReceivedCalls());
        }

        [Fact(DisplayName = "Remove")]
        [Trait("Remove", "Should Loggin Error")]
        public void Remove_Should_Loggin_Error()
        {
            // Arrange
            var key = "key";
            _memoryCache.When(x => x.Remove(Arg.Any<string>())).Do(x => { throw new Exception(); });

            // Act
            _sut.Remove(key);

            // Assert
            Assert.Single(_memoryCache.ReceivedCalls());
            Assert.Single(_logger.ReceivedCalls());
        }
    }
}