using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TaskManagement.Persistence.Configuration;

internal class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
    {
        builder.ToTable("Tasks", "tasks");

        builder.HasKey(e => e.TaskId);

        builder.Property(e => e.TaskId).UseIdentityColumn();
        builder.Property(e => e.TaskName).IsRequired();
        builder.Property(e => e.Description).IsRequired();

        builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(e => e.UpdatedOn).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(e => e.TaskStatus)
            .WithMany()
            .HasForeignKey(e => e.Status);
    }
}
