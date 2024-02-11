using MediatR;
using Microsoft.EntityFrameworkCore;

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

    public DeleteEntityCommandHandler(TDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteEntityCommand<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().Remove(request.Entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}