using Microsoft.EntityFrameworkCore;
using TaskAssignment.Domain.Assignments;

namespace TaskAssignment.Infrastructure.Database.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly TaskAssignmentDbContext _dbContext;

        public AssignmentRepository(TaskAssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyTaskAssignedToOtherUserAsync(IEnumerable<Guid> taskIds, Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserTaskAssignments.AnyAsync(a => taskIds.Contains(a.TaskId) && a.UserId != userId, cancellationToken);
        }

    }
}
