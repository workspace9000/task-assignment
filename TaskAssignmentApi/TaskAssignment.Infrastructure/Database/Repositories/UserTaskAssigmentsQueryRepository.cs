using Microsoft.EntityFrameworkCore;
using TaskAssignment.Application.Assignments;

namespace TaskAssignment.Infrastructure.Database.Repositories;

public class UserTaskAssigmentsQueryRepository : IUserTaskAssigmentsQueryRepository
{
    private readonly TaskAssignmentDbContext _dbContext;

    public UserTaskAssigmentsQueryRepository(TaskAssignmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ListAssignedTasksForUserItem>> GetAssignedTasksForUserAsync(ListAssignedTasksForUserQuery query, CancellationToken cancellationToken)
    {
        var result = (
                from a in _dbContext.UserTaskAssignments
                join t in _dbContext.TaskItems on a.TaskId equals t.Id
                where a.UserId == query.UserId
                orderby t.Difficulty descending, t.Id
                select new ListAssignedTasksForUserItem
                {
                    Id = t.Id,
                    Difficulty = t.Difficulty,
                    Type = t.Type.ToString(),
                    Status = t.Status.ToString()
                }
            )
            .Skip(query.Page * 10)
            .Take(10)
            .ToListAsync(cancellationToken);

        var tt = await result;

        return tt;
    }
}
