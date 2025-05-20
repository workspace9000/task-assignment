namespace TaskAssignment.Application.Tasks;

public interface ITaskQueryRepository
{
    Task<List<ListAvailableTasksForUserItem>> GetAvailableTasksForUserAsync(ListAvailableTasksForUserQuery queryParams, CancellationToken cancellationToken);
}