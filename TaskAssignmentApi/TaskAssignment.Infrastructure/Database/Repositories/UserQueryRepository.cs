using Microsoft.EntityFrameworkCore;
using TaskAssignment.Application.Users;

namespace TaskAssignment.Infrastructure.Database.Repositories;

public class UserQueryRepository : IUserQueryRepository
{
    private readonly TaskAssignmentDbContext _dbContext;

    public UserQueryRepository(TaskAssignmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ListAllUsersItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Select(u => new ListAllUsersItem
            {
                Id = u.Id,
                Name = u.Name,
                Role = u.Role.ToString()
            })
            .ToListAsync(cancellationToken);
    }
}

