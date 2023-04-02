using TaskManagement.Domain.Repositories;
using TaskManagement.Persistence.Context;

namespace TaskManagement.Persistence.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly TaskManagementDbContext _dbContext;

    public RepositoryBase(TaskManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }
}
