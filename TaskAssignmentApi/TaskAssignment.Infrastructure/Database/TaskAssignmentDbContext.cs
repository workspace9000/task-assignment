using Microsoft.EntityFrameworkCore;
using TaskAssignment.Domain.Assignments;
using TaskAssignment.Domain.Tasks;
using TaskAssignment.Domain.Users;
using TaskAssignment.Infrastructure.Database.Seed;

namespace TaskAssignment.Infrastructure.Database
{
    public class TaskAssignmentDbContext : DbContext
    {
#pragma warning disable CS8618 // Entity Framework takes care of that.
        public TaskAssignmentDbContext(DbContextOptions<TaskAssignmentDbContext> options) : base(options)
#pragma warning restore CS8618 // Entity Framework takes care of that.
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskAssignmentDbContext).Assembly);

            //modelBuilder.Ignore<TaskDetails>();

            MockDataSeeder.Seed(modelBuilder);
        }

        public DbSet<User> Users { set; get; }
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<UserTaskAssignment> UserTaskAssignments => Set<UserTaskAssignment>();

        public DbSet<ImplementationDetails> ImplementationDetails => Set<ImplementationDetails>();
        public DbSet<DeploymentDetails> DeploymentDetails => Set<DeploymentDetails>();
        public DbSet<MaintenanceDetails> MaintenanceDetails => Set<MaintenanceDetails>();
    }
}
