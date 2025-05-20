using Microsoft.EntityFrameworkCore;
using TaskAssignment.Domain.Users;

namespace TaskAssignment.Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskAssignmentDbContext _dbContext;

        public UserRepository(TaskAssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(u => u.Assignments)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<IEnumerable<Guid>> GetAllAssignedTaskIdsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserTaskAssignments
                .Select(a => a.TaskId)
                .ToListAsync(cancellationToken);
        }
    }
}
