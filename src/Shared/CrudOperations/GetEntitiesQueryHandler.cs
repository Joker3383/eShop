using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Shared.CrudOperations;

public class GetEntitiesQuery<TEntity,TDbContext > : IRequest<IQueryable<TEntity>>
    where TEntity : class, IEntity<int> where TDbContext : DbContext{ }

public class GetEntitiesQueryHandler<TEntity, TDbContext> : IRequestHandler<GetEntitiesQuery<TEntity, TDbContext>, IQueryable<TEntity>>
    where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public GetEntitiesQueryHandler(TDbContext context)
    {
        _context = context;
    }
    
    public Task<IQueryable<TEntity>> Handle(GetEntitiesQuery<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Set<TEntity>().AsNoTracking());
    }
}