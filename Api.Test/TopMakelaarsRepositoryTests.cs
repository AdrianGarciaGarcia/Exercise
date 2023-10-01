using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Repositories;
using Api.Services;
using Api.Services.FundaApi;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Api.Test
{
    public class Tests
    {
        public class TopMakelaarsRepositoryTests
        {
            private readonly Mock<IApiService> _mockApiService;
            private readonly Mock<IMemoryCache> _mockMemoryCache;
            private readonly TopMakelaarsRepository _repository;

            public TopMakelaarsRepositoryTests()
            {
                _mockApiService = new Mock<IApiService>();
                _mockMemoryCache = new Mock<IMemoryCache>();
                _repository = new TopMakelaarsRepository(_mockApiService.Object, _mockMemoryCache.Object);
            }

            [Fact]
            public async Task GetTopMakelaarsAsync_ShouldHandleLimitExceededException()
            {
                // Arrange
                _mockApiService.Setup(api => api.GetHousesOnSaleAsync(It.IsAny<List<string>>(), It.IsAny<int>()))
                               .Throws(new LimitExceededException("test message"));

                object dummyApiResponse;
                _mockMemoryCache.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out dummyApiResponse))
                               .Returns(false); 

                // Act
                var result = await _repository.GetTopMakelaarsAsync(new List<string>(), 2);

                // Assert
                Assert.True(result.IncompleteData);
            }
        }
    }
}