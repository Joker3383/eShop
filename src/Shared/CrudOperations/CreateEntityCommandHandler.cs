using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Shared.CrudOperations;

public class CreateEntityCommand<TEntity, TDbContext> : IRequest where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    public TEntity Entity { get; }

    public CreateEntityCommand(TEntity entity)
    {
        Entity = entity;
    }
}

public class CreateEntityCommandHandler<TEntity, TDbContext> : IRequestHandler<CreateEntityCommand<TEntity, TDbContext>> where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public CreateEntityCommandHandler(TDbContext context)
    {
        _context = context;
    }
    
    public async Task Handle(CreateEntityCommand<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().Add(request.Entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}