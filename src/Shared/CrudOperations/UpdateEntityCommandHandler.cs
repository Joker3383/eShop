using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.CrudOperations;

public class UpdateEntityCommand<TEntity, TDbContext> : IRequest<TEntity> where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    public TEntity Entity { get; }
    
    public UpdateEntityCommand(TEntity entity)
    {
        Entity = entity;
    }
}

public class UpdateEntityCommandHandler<TEntity, TDbContext> : IRequestHandler<UpdateEntityCommand<TEntity, TDbContext>, TEntity>
    where TEntity : class, IEntity<int> 
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly ILogger<UpdateEntityCommandHandler<TEntity, TDbContext>> _logger;

    public UpdateEntityCommandHandler(TDbContext context, ILogger<UpdateEntityCommandHandler<TEntity, TDbContext>> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<TEntity> Handle(UpdateEntityCommand<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, request.Entity.Id);

            var updateResult = _context.Set<TEntity>().Update(request.Entity);
            await _context.SaveChangesAsync(cancellationToken);
            return updateResult.Entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, request.Entity.Id);
            throw; 
        }
    }
}
