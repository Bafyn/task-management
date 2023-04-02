using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Repositories;
using TaskManagement.Persistence.Context;

namespace TaskManagement.Persistence.Repositories;

public class TaskRepository : RepositoryBase<Domain.Entities.Task>, ITaskRepository
{
    public TaskRepository(TaskManagementDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Domain.Entities.Task> GetTaskAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskId == id, cancellationToken);
    }

    public async Task<List<Domain.Entities.Task>> GetAllTasksAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Tasks.ToListAsync(cancellationToken);
    }
}
