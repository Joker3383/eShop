

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Order.API.Models.Dto;
using Order.API.Repositories;
using Xunit;

namespace Order.API.UnitTests.Repositories
{
    public class BasketRepositoryTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;

        public BasketRepositoryTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task GetBasketsAsync_ValidSubId_ReturnsBasketDto_Success()
        {
            // Arrange
            var subId = 123;
            var expectedBasketDto = new BasketDto { Id = 1, SubId = 4, TotalCount = 50, Products = null};
            var responseContent = JsonConvert.SerializeObject(new ResponseDto { IsSuccess = true, Result = expectedBasketDto });

            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(handler.Object);
            _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var repository = new BasketRepository(_httpClientFactoryMock.Object);

            // Act
            var result = await repository.GetBasketsAsync(subId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBasketDto, result);
        }

        [Fact]
        public async Task GetBasketsAsync_ValidSubId_ReturnsNull_Failure()
        {
            // Arrange
            var subId = 123;
            var responseContent = JsonConvert.SerializeObject(new ResponseDto { IsSuccess = false });

            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(handler.Object);
            _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var repository = new BasketRepository(_httpClientFactoryMock.Object);

            // Act
            var result = await repository.GetBasketsAsync(subId);

            // Assert
            Assert.Null(result);
        }
    }
}
