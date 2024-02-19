
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

            var subId = 123;
            var basketDto = new BasketDto { Id = 1, SubId = 4, TotalCount = 50, Products = null};
            var orderDto = new OrderDto { Id = 1, DateOfOrder = DateTime.Now, TotalSum = 45, BasketId = 1, SubId = 1};
            _basketRepositoryMock.Setup(repo => repo.GetBasketsAsync(subId)).ReturnsAsync(basketDto);
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateEntityCommand<Models.Order, AppDbContext>>(), default)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<OrderDto>(It.IsAny<Models.Order>())).Returns(orderDto);

            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);
            
            var result = await service.CreateOrder(subId);


            Assert.NotNull(result);
            Assert.IsType<OrderDto>(result);

        }

        [Fact]
        public async Task CreateOrder_InvalidSubId_ThrowsException_Failure()
        {

            var subId = 123;
            _basketRepositoryMock.Setup(repo => repo.GetBasketsAsync(subId)).ReturnsAsync((BasketDto)null);
            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);


            await Assert.ThrowsAsync<NullReferenceException>(() => service.CreateOrder(subId));
        }
        
        [Fact]
        public async Task DeleteOrder_ValidOrderId_ReturnsOrderDto_Success()
        {
            var orderId = 123;
            var order = new Models.Order { Id = 1, DateOfOrder = DateTime.Now, TotalSum = 45, BasketId = 1, SubId = 1};
            var orderDto = new OrderDto { Id = 1, DateOfOrder = DateTime.Now, TotalSum = 45, BasketId = 1, SubId = 1 };

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetEntityByIdQuery<Models.Order, AppDbContext>>(), default)).ReturnsAsync(order);
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<DeleteEntityCommand<Models.Order, AppDbContext>>(), default)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<OrderDto>(order)).Returns(orderDto);

            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);


            var result = await service.DeleteOrder(orderId);


            Assert.NotNull(result);
            Assert.IsType<OrderDto>(result);
        }

        [Fact]
        public async Task DeleteOrder_InvalidOrderId_ThrowsException_Failure()
        {

            var subId = 123;

            _basketRepositoryMock.Setup(repo => repo.GetBasketsAsync(subId)).ReturnsAsync((BasketDto)null);

            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);


            await Assert.ThrowsAsync<MediatorException>(() => service.DeleteOrder(subId));
        }
        
        [Fact]
        public async Task GetOrders_FindOrderById_ReturnsOrders_Success()
        {
            int subId = 123;
            var order = new Models.Order()
                { SubId = 123, DateOfOrder = DateTime.Now, TotalSum = 0, BasketId = 1, Id = 1 };
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetEntityBySubIdQuery<Models.Order, AppDbContext>>(), default)).ReturnsAsync(order);
            
            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);
            
            Assert.NotNull(() => service.DeleteOrder(subId));
        }

        [Fact]
        public async Task GetOrders_FindOrderById_ReturnsOrders_Failure()
        {
            int subId = 123;

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetEntityBySubIdQuery<Models.Order, AppDbContext>>(), default)).ReturnsAsync((Models.Order)null);
            
            var service = new OrderService(_basketRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);
            
            await Assert.ThrowsAsync<MediatorException>(() => service.DeleteOrder(subId));
        }
        
    }
}
