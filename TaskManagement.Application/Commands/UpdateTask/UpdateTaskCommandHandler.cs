using AutoMapper;
using MediatR;
using TaskManagement.Application.Services.Contracts;

namespace TaskManagement.Application.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UpdateTaskCommandResponse>
{
    private readonly IServiceBusProducer _serviceBusProducer;
    private readonly IMapper _mapper;

    public UpdateTaskCommandHandler(
        IServiceBusProducer serviceBusProducer,
        IMapper mapper)
    {
        _serviceBusProducer = serviceBusProducer;
        _mapper = mapper;
    }

    public Task<UpdateTaskCommandResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var updateTaskCommand = _mapper.Map<Messages.Commands.UpdateTaskCommand>(request);

        _serviceBusProducer.SendMessage(updateTaskCommand);

        return Task.FromResult(new UpdateTaskCommandResponse());
    }
}
