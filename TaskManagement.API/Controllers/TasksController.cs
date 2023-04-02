using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Commands.AddTask;
using TaskManagement.Application.Commands.UpdateTask;
using TaskManagement.Application.Queries.GetTasks;

namespace TaskManagement.API.Controllers;

// NOTE: A set of view model classes can be added to support controller endpoints return types
// In order to make the implementation simple classes from request handlers are reused.

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetTasksQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTasks(CancellationToken cancellationToken)
    {
        var query = new GetTasksQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddTaskCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTask(AddTaskCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(UpdateTaskCommandResponse), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        command = command with { Id = id };

        var result = await _mediator.Send(command, cancellationToken);

        return Accepted(result);
    }
}
