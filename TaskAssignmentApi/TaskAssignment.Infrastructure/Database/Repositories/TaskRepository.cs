using Microsoft.EntityFrameworkCore;
using TaskAssignment.Domain.Tasks;

namespace TaskAssignment.Infrastructure.Database.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskAssignmentDbContext _dbContext;

        public TaskRepository(TaskAssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.TaskItems.ToListAsync(cancellationToken);
        }
    }
}
