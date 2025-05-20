using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAssignment.Domain.Tasks;

namespace TaskAssignment.Infrastructure.Database.EntitiesConfigurations;

public class MaintenanceDetailsConfiguration : IEntityTypeConfiguration<MaintenanceDetails>
{
    public void Configure(EntityTypeBuilder<MaintenanceDetails> builder)
    {
        builder.HasKey(e => e.TaskId);

        builder.Property(e => e.Deadline)
            .IsRequired();

        builder.Property(e => e.Services)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(e => e.Servers)
            .IsRequired()
            .HasMaxLength(400);

        builder.HasOne<TaskItem>()
            .WithOne()
            .HasForeignKey<MaintenanceDetails>(e => e.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}