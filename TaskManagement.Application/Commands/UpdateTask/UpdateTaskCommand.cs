using MediatR;

namespace TaskManagement.Application.Commands.UpdateTask;

public record UpdateTaskCommand(int Id, int Status, string UpdatedBy) : IRequest<UpdateTaskCommandResponse>;
