﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManagement.Persistence.Context;

#nullable disable

namespace TaskManagement.Persistence.Migrations
{
    [DbContext(typeof(TaskManagementDbContext))]
    [Migration("20230402104634_AddTaskStatuses")]
    partial class AddTaskStatuses
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskManagement.Domain.Entities.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskId"));

                    b.Property<string>("AssignedTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TaskId");

                    b.HasIndex("Status");

                    b.ToTable("Tasks", "tasks");
                });

            modelBuilder.Entity("TaskManagement.Domain.Entities.TaskStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TaskStatuses", "tasks");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Description = "Task has been created",
                            Name = "NotStarted"
                        },
                        new
                        {
                            Id = 1,
                            Description = "Task is in progress",
                            Name = "InProgress"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Task has been completed",
                            Name = "Completed"
                        });
                });

            modelBuilder.Entity("TaskManagement.Domain.Entities.Task", b =>
                {
                    b.HasOne("TaskManagement.Domain.Entities.TaskStatus", "TaskStatus")
                        .WithMany()
                        .HasForeignKey("Status")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
