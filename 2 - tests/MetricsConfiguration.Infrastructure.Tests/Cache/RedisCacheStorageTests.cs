using MetricsConfiguration.Infrastructure.Cache;
using MetricsConfiguration.Infrastructure.Environment;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MetricsConfiguration.Infrastructure.Tests.Cache
{
    public class RedisCacheStorageTests
    {
        private readonly RedisCacheStorage _sut;
        private readonly IDistributedCache _redisCache;
        private readonly CacheConfigs _cacheConfigs;
        private readonly ILogger<RedisCacheStorage> _logger;

        public RedisCacheStorageTests()
        {
            _cacheConfigs = new CacheConfigs() { MemoryCache = new Environment.MemoryCache() { Key = "Mock", Expriration_Minutes = 1 }, RedisCache = new RedisCache() { Expriration_Minutes = 1, Key = "Mock", Url = "Url" } };
            _redisCache = Substitute.For<IDistributedCache>();
            _logger = Substitute.For<ILogger<RedisCacheStorage>>();
            _sut = Substitute.For<RedisCacheStorage>(_redisCache, _cacheConfigs, _logger);
        }

        [Fact(DisplayName = "GetOrCreateAsync")]
        [Trait("GetOrCreateAsync", "Should Return Data Of Redis Cache")]
        public async Task GetOrCreateAsync_Should_Return_Data_Of_RedisCache()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            var timeExpiration = DateTimeOffset.Now.AddSeconds(10);
            byte[] byteResponse = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));

            _redisCache.GetAsync(Arg.Any<string>()).Returns(byteResponse);

            // Act
            var result = await _sut.GetOrCreateAsync(key, () => Task.FromResult(data), timeExpiration);

            // Assert
            Assert.Single(_redisCache.ReceivedCalls());
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "GetOrCreateAsync")]
        [Trait("GetOrCreateAsync", "Should Execute Function And Store Result In Redis Cache")]
        public async Task GetOrCreateAsync_Should_Execute_Function_And_Store_Result_In_Redis_Cache()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            var timeExpiration = DateTimeOffset.Now.AddSeconds(10);
            byte[] byteResponse = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(null));

            _redisCache.GetAsync(Arg.Any<string>()).Returns(byteResponse);
            _redisCache.Set(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>());

            // Act
            var result = await _sut.GetOrCreateAsync(key, () => Task.FromResult(data), timeExpiration);

            // Assert
            Assert.Equal(2, _redisCache.ReceivedCalls().Count());
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "GetOrCreateAsync")]
        [Trait("GetOrCreateAsync", "Should LogError In Get RedisCache And Execute Function And Store Result In Redis Cache")]
        public async Task GetOrCreateAsync_ShouldThrowsException_And_LogginErrorAsync()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            var timeExpiration = DateTimeOffset.Now.AddSeconds(10);
            byte[] byteResponse = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));

            _redisCache.When(x => x.GetAsync(Arg.Any<string>())).Do(x => { throw new Exception(); });
            _redisCache.Set(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>());

            // Act
            var result = await _sut.GetOrCreateAsync(key, () => Task.FromResult(data), timeExpiration);

            // Assert
            Assert.Equal(2, _redisCache.ReceivedCalls().Count());
            Assert.NotNull(result);
            Assert.Single(_logger.ReceivedCalls());
        }

        [Fact(DisplayName = "GetOrCreateAsync")]
        [Trait("GetOrCreateAsync", "Should LogError 2 Errors And Execute Function And Not Store Result In Redis Cache")]
        public async Task GetOrCreateAsync_Should_LogError_2_Errors_And_Execute_Function_And_Not_Store_Result_In_Redis_Cache()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            var timeExpiration = DateTimeOffset.Now.AddSeconds(10);
            byte[] byteResponse = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));

            _redisCache.When(x => x.GetAsync(Arg.Any<string>())).Do(x => { throw new Exception(); });
            _redisCache.When(x => x.Set(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>())).Do(x => { throw new Exception(); });

            // Act
            var result = await _sut.GetOrCreateAsync(key, () => Task.FromResult(data), timeExpiration);

            // Assert
            Assert.Equal(2, _redisCache.ReceivedCalls().Count());
            Assert.NotNull(result);
            Assert.Equal(2, _logger.ReceivedCalls().Count());
        }

        [Fact(DisplayName = "Set")]
        [Trait("Set", "Should Store Data In Redis Cache, Without Expiration Date")]
        public void Set_ShouldStoreData_In_RedisCache_WithoutExpirationDate()
        {
            // Arrange
            var key = "key";
            var data = new { data = "data" };

            // Act
            _sut.Set(key, data);

            // Assert
            Assert.Single(_redisCache.ReceivedCalls());
        }

        [Fact(DisplayName = "Set")]
        [Trait("Set", "Should Store Data In Redis Cache With Expiration Date")]
        public void Set_ShouldStoreData_In_RedisCache_WithExpirationDate()
        {

            // Arrange
            var key = "key";
            var data = new { data = "data" };
            var timeOffset = DateTimeOffset.Now.AddSeconds(10);

            // Act
            _sut.Set(key, data, timeOffset);

            // Assert
            Assert.Single(_redisCache.ReceivedCalls());

        }

        [Fact(DisplayName = "Set")]
        [Trait("Set", "Should Throws Exception And Loggin Error")]
        public void Set_ShouldThrowsException_And_LogginError()
        {
            // Arrange
            var key = "key";
            var data = new { data = "data" };
            var timeExpiration = DateTimeOffset.Now.AddSeconds(10);
            _redisCache.When(x => x.Set(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>())).Do(x => { throw new Exception(); });

            // Act
            _sut.Set(key, data, timeExpiration);

            // Assert
            Assert.Single(_logger.ReceivedCalls());
        }


        [Fact(DisplayName = "GetAsync")]
        [Trait("GetAsync", "Should Get Data In Redis Cache")]
        public async Task GetAsync_Should_Get_Data_In_Redis_CacheAsync()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            byte[] byteResponse = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));
            _redisCache.GetAsync(Arg.Any<string>()).Returns(byteResponse);

            // Act
            var result = await _sut.GetAsync<object>(key);

            // Assert
            Assert.Single(_redisCache.ReceivedCalls());
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "GetAsync")]
        [Trait("GetAsync", "Should Return Null Object And Loggin Error")]
        public async Task GetAsync_Should_Return_Null_Object_And_Loggin_Error()
        {
            // Arrange
            var key = "key";
            var data = new { data = "Data Storage" };
            byte[] byteResponse = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));
            _redisCache.When(x => x.GetAsync(Arg.Any<string>())).Do(x => { throw new Exception(); });
            //_redisCache.GetAsync(Arg.Any<string>()).Returns(byteResponse);

            // Act
            var result = await _sut.GetAsync<object>(key);

            // Assert
            Assert.Single(_redisCache.ReceivedCalls());
            Assert.Null(result);
            Assert.Single(_logger.ReceivedCalls());
        }


        [Fact(DisplayName = "Remove")]
        [Trait("Remove", "Should Remove Index Of Redis Cache")]
        public void Remove_Should_Remove_Index_Of_Redis_Cache()
        {
            // Arrange
            var key = "key";

            // Act
            _sut.Remove(key);

            // Assert
            Assert.Single(_redisCache.ReceivedCalls());
        }

        [Fact(DisplayName = "Remove")]
        [Trait("Remove", "Should Loggin Error")]
        public void Remove_Should_Loggin_Error()
        {
            // Arrange
            var key = "key";
            _redisCache.When(x => x.Remove(Arg.Any<string>())).Do(x => { throw new Exception(); });

            // Act
            _sut.Remove(key);

            // Assert
            Assert.Single(_redisCache.ReceivedCalls());
            Assert.Single(_logger.ReceivedCalls());
        }

    }
}