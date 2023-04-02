using MediatR;

namespace TaskManagement.Application.Queries.GetTasks;

public record GetTasksQuery() : IRequest<GetTasksQueryResponse>;
