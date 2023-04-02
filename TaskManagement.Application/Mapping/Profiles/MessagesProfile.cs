using AutoMapper;
using TaskManagement.Application.Commands.UpdateTask;

namespace TaskManagement.Application.Mapping.Profiles;

public class MessagesProfile : Profile
{
    public MessagesProfile()
    {
        CreateMap<UpdateTaskCommand, Messages.Commands.UpdateTaskCommand>();
    }
}
