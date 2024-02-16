
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.CrudOperations;

public class GetEntityBySubIdQuery<TEntity, TDbContext> : IRequest<TEntity?> where TEntity : class, IEntityWithSubId<int> where TDbContext : DbContext
{

    
    public int SubId { get; }

    public GetEntityBySubIdQuery(int subId)
    {

        SubId = subId;
    }
}

public class GetEntityBySubIdQueryHandler<TEntity, TDbContext> : IRequestHandler<GetEntityBySubIdQuery<TEntity, TDbContext>, TEntity?>
    where TEntity : class, IEntityWithSubId<int> 
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly ILogger<GetEntityBySubIdQueryHandler<TEntity, TDbContext>> _logger;

    public GetEntityBySubIdQueryHandler(TDbContext context, ILogger<GetEntityBySubIdQueryHandler<TEntity, TDbContext>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TEntity?> Handle(GetEntityBySubIdQuery<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, request.SubId);

            return await _context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.SubId == request.SubId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, request.SubId);
            throw; 
        }
    }
}


