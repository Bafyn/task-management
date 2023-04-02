using AutoMapper;
using TaskManagement.Application.Commands.AddTask;

namespace TaskManagement.Application.Mapping.Profiles;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<AddTaskCommand, Domain.Entities.Task>()
            .ForMember(d => d.TaskName, cfg => cfg.MapFrom(s => s.Name));

        CreateMap<Domain.Entities.Task, Contracts.Models.Task>()
            .ForMember(d => d.Id, cfg => cfg.MapFrom(s => s.TaskId))
            .ForMember(d => d.Name, cfg => cfg.MapFrom(s => s.TaskName));
    }
}
