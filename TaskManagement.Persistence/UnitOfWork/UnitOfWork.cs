using TaskManagement.Persistence.Context;

namespace TaskManagement.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskManagementDbContext _dbContext;

    public UnitOfWork(TaskManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
