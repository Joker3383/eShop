using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.CrudOperations;

public class DeleteEntityCommand<TEntity, TDbContext> : IRequest where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    public TEntity Entity { get; }

    public DeleteEntityCommand(TEntity entity)
    {
        Entity = entity;
    }
}

public class DeleteEntityCommandHandler<TEntity, TDbContext> : IRequestHandler<DeleteEntityCommand<TEntity, TDbContext>> where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private  readonly ILogger<DeleteEntityCommandHandler<TEntity, TDbContext>> _logger;

    public DeleteEntityCommandHandler(TDbContext context,ILogger<DeleteEntityCommandHandler<TEntity, TDbContext>> logger )
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(DeleteEntityCommand<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        try
        {
            var result = _context.Set<TEntity>().Remove(request.Entity);
            if (result != null)
            {
            
                var deletedRes = await _context.SaveChangesAsync(cancellationToken);
                if(deletedRes == 0)
                    _logger.LogInformation($"Error: {typeof(TEntity)} wasn`t deleted" );
                _logger.LogInformation($"Info: {typeof(TEntity)} was deleted and saved" );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while adding entity of type {typeof(TEntity)}");
            throw; 
        }
    }
}