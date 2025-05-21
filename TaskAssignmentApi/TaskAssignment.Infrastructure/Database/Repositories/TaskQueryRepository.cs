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

    public async Task<ListAvailableTasksForUser> GetAvailableTasksForUserAsync(ListAvailableTasksForUserQuery queryParams, CancellationToken cancellationToken)
    {
        const int PageSize = 10;

        var baseQuery =
            from t in _dbContext.TaskItems
            join a in _dbContext.UserTaskAssignments on t.Id equals a.TaskId into assigned
            from a in assigned.DefaultIfEmpty()
            join u in _dbContext.Users on queryParams.UserId equals u.Id
            where a == null && (
                (u.Role == UserRoles.Developer && t.Type == TaskTypes.Implementation) ||
                (u.Role == UserRoles.DevOps || u.Role == UserRoles.Administrator)
            )
            select t;

        var totalCount = await baseQuery.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        var items = await baseQuery
            .OrderByDescending(t => t.Difficulty).ThenBy(t => t.Id)
            .Skip(queryParams.Page * PageSize)
            .Take(PageSize)
            .Select(t => new ListAvailableTasksForUserItem
            {
                Id = t.Id,
                Difficulty = t.Difficulty,
                Type = t.Type.ToString(),
                Status = t.Status.ToString()
            })
            .ToListAsync(cancellationToken);

        return new ListAvailableTasksForUser
        {
            Items = items,
            TotalPages = totalPages
        };
    }
}
