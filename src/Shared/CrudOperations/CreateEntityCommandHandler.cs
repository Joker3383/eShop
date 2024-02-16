using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.CrudOperations;

public class CreateEntityCommand<TEntity, TDbContext> : IRequest where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    public TEntity Entity { get; }

    public CreateEntityCommand(TEntity entity)
    {
        Entity = entity;
    }
}

public class CreateEntityCommandHandler<TEntity, TDbContext> : IRequestHandler<CreateEntityCommand<TEntity, TDbContext>> 
    where TEntity : class, IEntity<int> 
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly ILogger<CreateEntityCommandHandler<TEntity, TDbContext>> _logger;

    public CreateEntityCommandHandler(TDbContext context, ILogger<CreateEntityCommandHandler<TEntity, TDbContext>> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task Handle(CreateEntityCommand<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        try
        {
            var result = _context.Set<TEntity>().Add(request.Entity);
            if (result != null)
            {
                var saveRes = await _context.SaveChangesAsync(cancellationToken);
                if(saveRes == 0)
                    _logger.LogInformation($"Error: {typeof(TEntity)} wasn't saved");
                else
                    _logger.LogInformation($"Info: {typeof(TEntity)} was added and saved");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while adding entity of type {typeof(TEntity)}");
            throw; 
        }
    }
}
