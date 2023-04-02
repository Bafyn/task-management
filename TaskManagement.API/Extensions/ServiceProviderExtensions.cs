using Microsoft.EntityFrameworkCore;
using TaskManagement.Persistence.Context;

namespace TaskManagement.API.Extensions;

internal static class ServiceProviderExtensions
{
    public static async Task ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
