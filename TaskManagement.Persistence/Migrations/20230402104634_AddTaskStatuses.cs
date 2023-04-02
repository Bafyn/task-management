using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "tasks",
                table: "TaskStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 0, "Task has been created", "NotStarted" },
                    { 1, "Task is in progress", "InProgress" },
                    { 2, "Task has been completed", "Completed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "tasks",
                table: "TaskStatuses",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                schema: "tasks",
                table: "TaskStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "tasks",
                table: "TaskStatuses",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
