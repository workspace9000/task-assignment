using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAssignment.Domain.Assignments;
using TaskAssignment.Domain.Tasks;
using TaskAssignment.Domain.Users;

namespace TaskAssignment.Infrastructure.Database.EntitiesConfigurations;

public class UserTaskAssignmentConfiguration : IEntityTypeConfiguration<UserTaskAssignment>
{
    public void Configure(EntityTypeBuilder<UserTaskAssignment> builder)
    {
        builder.HasKey(e => new { e.UserId, e.TaskId });

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<TaskItem>()
            .WithOne()
            .HasForeignKey<UserTaskAssignment>(e => e.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(e => e.TaskId)
            .IsUnique();
    }
}
