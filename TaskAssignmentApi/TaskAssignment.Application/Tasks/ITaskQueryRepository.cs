namespace TaskAssignment.Application.Tasks;

public interface ITaskQueryRepository
{
    Task<ListAvailableTasksForUser> GetAvailableTasksForUserAsync(ListAvailableTasksForUserQuery queryParams, CancellationToken cancellationToken);
}