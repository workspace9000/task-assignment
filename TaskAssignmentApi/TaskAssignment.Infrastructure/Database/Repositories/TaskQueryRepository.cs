using Microsoft.EntityFrameworkCore;
using TaskAssignment.Application.Tasks;
using TaskAssignment.Domain.Tasks;
using TaskAssignment.Domain.Users;

namespace TaskAssignment.Infrastructure.Database.Repositories;

public class TaskQueryRepository : ITaskQueryRepository
{
    private readonly TaskAssignmentDbContext _dbContext;

    public TaskQueryRepository(TaskAssignmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<ListAvailableTasksForUserItem>> GetAvailableTasksForUserAsync(ListAvailableTasksForUserQuery queryParams, CancellationToken cancellationToken)
    {
        var query =
            from t in _dbContext.TaskItems
            join a in _dbContext.UserTaskAssignments on t.Id equals a.TaskId into assigned
            from a in assigned.DefaultIfEmpty()
            join u in _dbContext.Users on queryParams.UserId equals u.Id
            where a == null && (
                (u.Role == UserRoles.Developer && t.Type == TaskTypes.Implementation) ||
                (u.Role == UserRoles.DevOps || u.Role == UserRoles.Administrator)
            )
            orderby t.Difficulty descending, t.Id
            select new ListAvailableTasksForUserItem
            {
                Id = t.Id,
                Difficulty = t.Difficulty,
                Type = t.Type.ToString(),
                Status = t.Status.ToString()
            };

        return query
            .Skip(queryParams.Page * 10)
            .Take(10)
            .ToListAsync(cancellationToken);
    }
}
