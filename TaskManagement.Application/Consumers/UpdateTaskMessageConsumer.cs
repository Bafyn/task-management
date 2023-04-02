using TaskManagement.Application.Consumers.Contracts;
using TaskManagement.Application.Exceptions;
using TaskManagement.Domain.Repositories;
using TaskManagement.Messages.Commands;
using TaskManagement.Persistence.UnitOfWork;

namespace TaskManagement.Application.Consumers;

public class UpdateTaskMessageConsumer : IMessageConsumer<UpdateTaskCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskMessageConsumer(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ConsumeAsync(UpdateTaskCommand message)
    {
        var task = await _taskRepository.GetTaskAsync(message.Id, CancellationToken.None);

        if (task == null)
        {
            throw new TaskNotFoundException(message.Id);
        }

        task.Status = message.Status;
        task.UpdatedBy = message.UpdatedBy;
        task.UpdatedOn = DateTime.UtcNow;

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }
}
