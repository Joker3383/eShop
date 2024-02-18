using Catalog.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.CrudOperations;

namespace Shared.UnitTests.CrudOperationTests;

public class GetEntitiesQueryHandlerTests<TEntity, TDbContext> 
    where TEntity : class, IEntity<int> where TDbContext : DbContext
    {
        private readonly Mock<TDbContext> _dbContextMock;
        private readonly Mock<ILogger<GetEntitiesQueryHandler<TEntity, TDbContext>>> _loggerMock;
        private readonly GetEntitiesQueryHandler<TEntity, TDbContext> _handler;

        public GetEntitiesQueryHandlerTests()
        {
            _dbContextMock = new Mock<TDbContext>();
            _loggerMock = new Mock<ILogger<GetEntitiesQueryHandler<TEntity, TDbContext>>>();
            _handler = new GetEntitiesQueryHandler<TEntity, TDbContext>(_dbContextMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnEntities_WhenRequestIsHandled()
        {
            // Arrange
            var entities = new List<IEntity<int>>
            {
                new Product { Id = 1 },
                new Basket.API.Models.Basket { Id = 2 },
                new Order.API.Models.Order { Id = 3 }
            }.AsQueryable();

            var dbSetMock = new Mock<DbSet<TEntity>>();
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            

            _dbContextMock.Setup(c => c.Set<TEntity>()).Returns(dbSetMock.Object);

            var request = new GetEntitiesQuery<TEntity, TDbContext>();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities.Count(), result.Count());
        }

        [Fact]
        public async Task Handle_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            var exceptionMessage = "Simulated exception";
            _dbContextMock.Setup(c => c.Set<TEntity>()).Throws(new System.Exception(exceptionMessage));

            var request = new GetEntitiesQuery<TEntity, TDbContext>();

            // Act + Assert
            await Assert.ThrowsAsync<System.Exception>(async () => await _handler.Handle(request, CancellationToken.None));
            _loggerMock.Verify(x => x.LogError(It.IsAny<System.Exception>(), It.IsAny<string>(), typeof(TEntity).Name), Times.Once);
        }

        public Task<IQueryable<TEntity>> Handle(GetEntitiesQuery<TEntity, TDbContext> request, CancellationToken cancellationToken)
        {
            
        }
    }