namespace TaskManagement.Domain.Repositories;

public interface ITaskRepository : IRepositoryBase<Entities.Task>
{
    Task<Entities.Task> GetTaskAsync(int id, CancellationToken cancellationToken);

    Task<List<Entities.Task>> GetAllTasksAsync(CancellationToken cancellationToken);
}
