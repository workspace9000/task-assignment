namespace TaskAssignment.Application.Users;

public interface IUserQueryRepository
{
    Task<List<ListAllUsersItem>> GetAllAsync(CancellationToken cancellationToken);
}


