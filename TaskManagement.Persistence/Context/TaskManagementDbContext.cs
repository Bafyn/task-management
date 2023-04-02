using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TaskManagement.Persistence.Context;

public class TaskManagementDbContext : DbContext
{
    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Task> Tasks { get; set; }

    public DbSet<Domain.Entities.TaskStatus> TaskStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
