using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.CrudOperations;

public class GetEntitiesQuery<TEntity,TDbContext > : IRequest<IQueryable<TEntity>>
    where TEntity : class, IEntity<int> where TDbContext : DbContext{ }

public class GetEntitiesQueryHandler<TEntity, TDbContext> : IRequestHandler<GetEntitiesQuery<TEntity, TDbContext>, IQueryable<TEntity>>
    where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly ILogger<GetEntitiesQueryHandler<TEntity, TDbContext>> _logger;

    public GetEntitiesQueryHandler(TDbContext context, ILogger<GetEntitiesQueryHandler<TEntity, TDbContext>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IQueryable<TEntity>> Handle(GetEntitiesQuery<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching entities of type {EntityType}", typeof(TEntity).Name);

            var entities = _context.Set<TEntity>().AsNoTracking();
            return entities;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching entities of type {EntityType}", typeof(TEntity).Name);
            throw; 
        }
    }
}