namespace TaskAssignment.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Guid>> GetAllAssignedTaskIdsAsync(CancellationToken cancellationToken = default);
}