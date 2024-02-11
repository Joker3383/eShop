using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Shared.CrudOperations;

public class GetEntityByIdQuery<TEntity, TDbContext> : IRequest<TEntity?> where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    public int Id { get; }

    public GetEntityByIdQuery(int id)
    {
        Id = id;
    }
}

public class GetEntityByIdQueryHandler<TEntity, TDbContext> : IRequestHandler<GetEntityByIdQuery<TEntity, TDbContext>, TEntity?> where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public GetEntityByIdQueryHandler(TDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> Handle(GetEntityByIdQuery<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}

// complited huinja 