using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAssignment.Domain.Tasks;

namespace TaskAssignment.Infrastructure.Database.EntitiesConfigurations;

public class ImplementationDetailsConfiguration : IEntityTypeConfiguration<ImplementationDetails>
{
    public void Configure(EntityTypeBuilder<ImplementationDetails> builder)
    {
        builder.HasKey(e => e.TaskId);

        builder.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne<TaskItem>()
            .WithOne()
            .HasForeignKey<ImplementationDetails>(e => e.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
