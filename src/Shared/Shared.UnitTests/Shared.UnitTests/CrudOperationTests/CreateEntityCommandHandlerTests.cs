using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.CrudOperations;

public class CreateEntityCommandHandlerTests<TEntity, TDbContext>
    where TEntity : class, IEntity<int>
    where TDbContext : DbContext
{
    /*[Fact]
    public async Task Handle_ValidEntity_SavesEntityToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        using (var context = new TDbContext(options))
        {
            var entity = new TEntity();
            var command = new CreateEntityCommand<TEntity, TDbContext>(entity);
            var loggerMock = new Mock<ILogger<CreateEntityCommandHandler<TEntity, TDbContext>>>();
            var handler = new CreateEntityCommandHandler<TEntity, TDbContext>(context, loggerMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var savedEntity = await context.Set<TEntity>().FirstOrDefaultAsync();
            Assert.NotNull(savedEntity);
            // Add more assertions as needed
        }
    }*/

    [Fact]
    public async Task Handle_InvalidEntity_DoesNotSaveEntityToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var context = new Mock<TDbContext>(options);
        
        var entityMock = new Mock<TEntity>();
        var dbContextMock = new Mock<TDbContext>();
        var command = new CreateEntityCommand<TEntity, TDbContext>(null);
        var loggerMock = new Mock<ILogger<CreateEntityCommandHandler<TEntity, TDbContext>>>();
        var handler = new CreateEntityCommandHandler<TEntity, TDbContext>(context.Object, loggerMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var savedEntity = await context.Object.Set<TEntity>().FirstOrDefaultAsync();
        Assert.Null(savedEntity);

        
    }
}
