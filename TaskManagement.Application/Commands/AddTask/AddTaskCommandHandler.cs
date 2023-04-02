using AutoMapper;
using MediatR;
using TaskManagement.Domain.Repositories;
using TaskManagement.Persistence.UnitOfWork;

namespace TaskManagement.Application.Commands.AddTask;

public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, AddTaskCommandResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddTaskCommandHandler(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AddTaskCommandResponse> Handle(AddTaskCommand request, CancellationToken cancellationToken)
    {
        // NOTE: Optionally validation can be added here or as a middleware extension for MediatR
        var task = _mapper.Map<Domain.Entities.Task>(request);

        // The default status for new tasks is 'NotStarted'.
        task.Status = (int)Contracts.Enums.TaskStatus.NotStarted;
        task.CreatedOn = DateTime.UtcNow;

        _taskRepository.Create(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddTaskCommandResponse(task.TaskId);
    }
}
