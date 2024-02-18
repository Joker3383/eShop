
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Moq;
using Order.API.Data;
using Order.API.Models;
using Order.API.Models.Dto;
using Order.API.Repositories.Interfaces;
using Order.API.Services;
using Shared.CrudOperations;
using Xunit;

namespace Order.API.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IBasketRepository> _basketRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;

        public OrderServiceTests()
        {
            _basketRepositoryMock = new Mock<IBasketRepository>();
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task CreateOrder_ValidSubId_ReturnsOrderDto_Success()
        {
            // Arrange
            var subId = 123;
            var basketDto = new BasketDto { Id = 1, SubId = 4, TotalCount = 50, Products = null};
            var order = new Models.Order { Id = 1, DateOfOrder = DateTime.Now, TotalSum = 45, BasketId = 1, SubId = 1};
            var orderDto = new OrderDto { Id = 1, DateOfOrder = DateTime.Now, TotalSum = 45, BasketId = 1, SubId = 1};

            _basketRepositoryMock.Setup(repo => repo.GetBasketsAsync(subId)).ReturnsAsync(basketDto);
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateEntityCommand<Models.Order, AppDbContext>>(), default)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<OrderDto>(It.IsAny<Models.Order>())).Returns(orderDto);

            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);

            // Act
            var result = await service.CreateOrder(subId);


            Assert.NotNull(result);
            Assert.IsType<OrderDto>(result);

        }

        [Fact]
        public async Task CreateOrder_InvalidSubId_ThrowsException_Failure()
        {
            // Arrange
            var subId = 123;

            _basketRepositoryMock.Setup(repo => repo.GetBasketsAsync(subId)).ReturnsAsync((BasketDto)null);

            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => service.CreateOrder(subId));
        }

        

        [Fact]
        public async Task DeleteOrder_ValidOrderId_ReturnsOrderDto_Success()
        {
            // Arrange
            var orderId = 123;
            var order = new Models.Order { Id = 1, DateOfOrder = DateTime.Now, TotalSum = 45, BasketId = 1, SubId = 1};
            var orderDto = new OrderDto { Id = 1, DateOfOrder = DateTime.Now, TotalSum = 45, BasketId = 1, SubId = 1 };

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetEntityByIdQuery<Models.Order, AppDbContext>>(), default)).ReturnsAsync(order);
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<DeleteEntityCommand<Models.Order, AppDbContext>>(), default)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<OrderDto>(order)).Returns(orderDto);

            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);

            // Act
            var result = await service.DeleteOrder(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderDto>(result);
            // Additional assertions if necessary
        }

        [Fact]
        public async Task DeleteOrder_InvalidOrderId_ThrowsException_Failure()
        {
           
        }
    }
}
