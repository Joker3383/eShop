using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Shared.CrudOperations;

public class UpdateEntityCommand<TEntity, TDbContext> : IRequest<TEntity> where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    public TEntity Entity { get; }
    
    public UpdateEntityCommand(TEntity entity)
    {
        Entity = entity;
    }
}

public class UpdateEntityCommandHandler<TEntity, TDbContext> : IRequestHandler<UpdateEntityCommand<TEntity, TDbContext>, TEntity> where TEntity : class, IEntity<int> where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public UpdateEntityCommandHandler(TDbContext context)
    {
        _context = context;
    }
    
    public async Task<TEntity> Handle(UpdateEntityCommand<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        var updateResult = _context.Set<TEntity>().Update(request.Entity);
        await _context.SaveChangesAsync(cancellationToken);
        return updateResult.Entity;
    }
}