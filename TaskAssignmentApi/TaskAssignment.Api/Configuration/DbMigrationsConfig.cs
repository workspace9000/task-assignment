using TaskAssignment.Infrastructure.Database;

namespace TaskAssignment.Api.Configuration;

public static class DbMigrationsConfig
{
    public static void ExecuteDbMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TaskAssignmentDbContext>();

        context.Database.EnsureCreated();
    }
}
