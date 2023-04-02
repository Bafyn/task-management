using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Entities = TaskManagement.Domain.Entities;

namespace TaskManagement.Persistence.Configuration;

internal class TaskStatusConfiguration : IEntityTypeConfiguration<Entities.TaskStatus>
{
    public void Configure(EntityTypeBuilder<Entities.TaskStatus> builder)
    {
        builder.ToTable("TaskStatuses", "tasks");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Name).IsRequired();

        builder.HasData(GetDefaultStatuses());
    }

    private static Entities.TaskStatus[] GetDefaultStatuses() => new Entities.TaskStatus[]
    {
        new() { Id = 0, Name = "NotStarted", Description = "Task has been created" },
        new() { Id = 1, Name = "InProgress", Description = "Task is in progress" },
        new() { Id = 2, Name = "Completed", Description = "Task has been completed" }
    };
}
