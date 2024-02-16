using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.CrudOperations;

public class GetEntitiesBySubIdQuery<TEntity, TDbContext> : IRequest<IQueryable<TEntity>>
    where TEntity : class, IEntityWithSubId<int> where TDbContext : DbContext
{
    public int SubId { get; set; }

    public GetEntitiesBySubIdQuery(int subId)
    {
        SubId = subId;
    }
}

public class GetEntitiesBySubIdQueryHandler<TEntity, TDbContext> : IRequestHandler<GetEntitiesBySubIdQuery<TEntity, TDbContext>, IQueryable<TEntity>>
    where TEntity : class, IEntityWithSubId<int> where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly ILogger<GetEntitiesBySubIdQueryHandler<TEntity, TDbContext>> _logger;

    public GetEntitiesBySubIdQueryHandler(TDbContext context, ILogger<GetEntitiesBySubIdQueryHandler<TEntity, TDbContext>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IQueryable<TEntity>> Handle(GetEntitiesBySubIdQuery<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching entities of type {EntityType}", typeof(TEntity).Name);

            var entities = _context.Set<TEntity>().Where(x => x.SubId == request.SubId);
            return entities;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching entities of type {EntityType}", typeof(TEntity).Name);
            throw; 
        }
    }
}