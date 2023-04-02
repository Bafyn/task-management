using MediatR;

namespace TaskManagement.Application.Commands.AddTask;

public record AddTaskCommand(string Name, string Description, string AssignedTo, string CreatedBy) : IRequest<AddTaskCommandResponse>;
