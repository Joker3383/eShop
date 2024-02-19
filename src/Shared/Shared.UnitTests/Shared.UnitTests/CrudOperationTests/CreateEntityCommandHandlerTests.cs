using Catalog.API.Data;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.CrudOperations;

public class CreateEntityCommandHandlerTests<TEntity, TDbContext>
    where TEntity : class, IEntity<int>
    where TDbContext : DbContext
{
    /*[Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var mockContext = new Mock<DbContext>();
        var mockTransaction = new Mock<IDbContextTransaction>();
        var mockDbSet = new Mock<DbSet<Product>>();
        var mockLogger = new Mock<ILogger<CreateEntityCommandHandler<Product, DbContext>>>();

        var entity = new EntityEntry<Product>(); // Sample entity
        var saveChangesResult = 1; // Simulating one entity saved

        mockContext.Setup(c => c.Database.BeginTransactionAsync(default))
            .ReturnsAsync(mockTransaction.Object);

        mockContext.Setup(c => c.Set<Product>())
            .Returns(mockDbSet.Object);

        mockDbSet.Setup(s => s.Add(entity))
            .Returns(entity); // Simulating adding entity to DbSet

        mockContext.Setup(c => c.SaveChangesAsync(default))
            .ReturnsAsync(saveChangesResult);

        var handler = new CreateEntityCommandHandler<Product, DbContext>(mockContext.Object, mockLogger.Object);
        var command = new CreateEntityCommand<Product, DbContext>(entity);

        // Act
        await handler.Handle(command, default);

        // Assert
        mockTransaction.Verify(t => t.CommitAsync(default), Times.Once);
        mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
    }*/

    
}
