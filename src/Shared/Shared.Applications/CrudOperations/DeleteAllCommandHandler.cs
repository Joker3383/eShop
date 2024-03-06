using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Shared.CrudOperations;

public class DeleteAllCommand<TEntity, TDbContext> : IRequest where TEntity : class, IEntityWithSubId<int> where TDbContext : DbContext
{
    public int SubId { get; }

    public DeleteAllCommand(int subId)
    {
        SubId = subId; 
    }
}
public class DeleteAllCommandHandler<TEntity, TDbContext> : IRequestHandler<DeleteAllCommand<TEntity, TDbContext>> where TEntity : class, IEntityWithSubId<int> where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly ILogger<DeleteAllCommandHandler<TEntity, TDbContext>> _logger;

    public DeleteAllCommandHandler(TDbContext context, ILogger<DeleteAllCommandHandler<TEntity, TDbContext>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(DeleteAllCommand<TEntity, TDbContext> request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var entitiesToDelete = await _context.Set<TEntity>().Where(o => o.SubId == request.SubId).ToListAsync();
            _context.Set<TEntity>().RemoveRange(entitiesToDelete);
            var deletedRes = await _context.SaveChangesAsync(cancellationToken);

            if (deletedRes == 0)
            {
                _logger.LogError($"Error: {typeof(TEntity)} wasn't deleted");
                await transaction.RollbackAsync();
            }
            else
            {
                await transaction.CommitAsync();
                _logger.LogInformation($"Info: {typeof(TEntity)} was deleted and saved");
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting entity of type {typeof(TEntity)}");
            await transaction.RollbackAsync();
            throw;
        }
    }
}



