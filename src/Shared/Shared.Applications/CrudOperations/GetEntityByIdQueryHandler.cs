using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.CrudOperations;

public class GetEntityByIdQuery<TEntity, TDbContext> : IRequest<TEntity?> where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    public int Id { get; }

    public GetEntityByIdQuery(int id)
    {
        Id = id;
    }
}

public class GetEntityByIdQueryHandler<TEntity, TDbContext> : IRequestHandler<GetEntityByIdQuery<TEntity, TDbContext>, TEntity?>
    where TEntity : class, IEntity<int> 
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly ILogger<GetEntityByIdQueryHandler<TEntity, TDbContext>> _logger;

    public GetEntityByIdQueryHandler(TDbContext context, ILogger<GetEntityByIdQueryHandler<TEntity, TDbContext>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TEntity?> Handle(GetEntityByIdQuery<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, request.Id);

            return await _context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, request.Id);
            throw; 
        }
    }
}


