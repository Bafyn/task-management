using AutoMapper;
using MediatR;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Queries.GetTasks;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public GetTasksQueryHandler(
        ITaskRepository taskRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<GetTasksQueryResponse> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetAllTasksAsync(cancellationToken);
        var taskModels = _mapper.Map<List<Contracts.Models.Task>>(tasks);

        return new GetTasksQueryResponse(taskModels);
    }
}
