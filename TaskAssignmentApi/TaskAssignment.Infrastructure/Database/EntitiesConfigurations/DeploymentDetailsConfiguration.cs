using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAssignment.Domain.Tasks;

namespace TaskAssignment.Infrastructure.Database.EntitiesConfigurations;

public class DeploymentDetailsConfiguration : IEntityTypeConfiguration<DeploymentDetails>
{
    public void Configure(EntityTypeBuilder<DeploymentDetails> builder)
    {
        builder.HasKey(e => e.TaskId);

        builder.Property(e => e.Deadline)
            .IsRequired();

        builder.Property(e => e.Scope)
            .IsRequired()
            .HasMaxLength(400);

        builder.HasOne<TaskItem>()
            .WithOne()
            .HasForeignKey<DeploymentDetails>(e => e.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}