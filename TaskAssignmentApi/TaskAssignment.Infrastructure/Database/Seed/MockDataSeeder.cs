using Microsoft.EntityFrameworkCore;
using TaskAssignment.Domain.Tasks;
using TaskAssignment.Domain.Users;

namespace TaskAssignment.Infrastructure.Database.Seed;

public static class MockDataSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var users = new List<User>
        {
            new("Alice", UserRoles.Developer),
            new("Bob", UserRoles.Developer),
            new("Carol", UserRoles.DevOps),
            new("David", UserRoles.DevOps),
            new("Eve", UserRoles.Administrator),
            new("Frank", UserRoles.Administrator),
        };

        modelBuilder.Entity<User>().HasData(users);

        var taskItems = new List<TaskItem>();
        var implementationDetails = new List<ImplementationDetails>();
        var deploymentDetails = new List<DeploymentDetails>();
        var maintenanceDetails = new List<MaintenanceDetails>();

        var rnd = new Random();
        int totalTasks = 55;
        for (int i = 0; i < totalTasks; i++)
        {
            var difficulty = rnd.Next(1, 6);
            var type = (TaskTypes)(i % 3);
            TaskDetails details;

            switch (type)
            {
                case TaskTypes.Implementation:
                    details = new ImplementationDetails($"Implementation task {i} details");
                    implementationDetails.Add((ImplementationDetails)details);
                    break;

                case TaskTypes.Deployment:
                    details = new DeploymentDetails(
                        DateTime.UtcNow.AddDays(rnd.Next(1, 30)),
                        $"Deployment scope {i}");
                    deploymentDetails.Add((DeploymentDetails)details);
                    break;

                case TaskTypes.Maintenance:
                    details = new MaintenanceDetails(
                        DateTime.UtcNow.AddDays(rnd.Next(1, 30)),
                        $"Service group {i}",
                        $"Server cluster {i}");
                    maintenanceDetails.Add((MaintenanceDetails)details);
                    break;

                default:
                    throw new InvalidOperationException();
            }

            var taskItem = new TaskItem(difficulty, type, TaskStatuses.ToDo);


            details.GetType().GetProperty("TaskId")!.SetValue(details, taskItem.Id);

            taskItems.Add(taskItem);
        }

        modelBuilder.Entity<TaskItem>().HasData(taskItems);
        modelBuilder.Entity<ImplementationDetails>().HasData(implementationDetails);
        modelBuilder.Entity<DeploymentDetails>().HasData(deploymentDetails);
        modelBuilder.Entity<MaintenanceDetails>().HasData(maintenanceDetails);
    }
}
