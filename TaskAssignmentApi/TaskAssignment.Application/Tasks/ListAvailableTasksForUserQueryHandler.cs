using MediatR;

namespace TaskAssignment.Application.Tasks;

public class ListAvailableTasksForUserQueryHandler : IRequestHandler<ListAvailableTasksForUserQuery, ListAvailableTasksForUser>
{
    private readonly ITaskQueryRepository _taskQueryRepository;

    public ListAvailableTasksForUserQueryHandler(ITaskQueryRepository taskQueryRepository)
    {
        _taskQueryRepository = taskQueryRepository;
    }

    public Task<ListAvailableTasksForUser> Handle(ListAvailableTasksForUserQuery query, CancellationToken cancellationToken)
    {
        return _taskQueryRepository.GetAvailableTasksForUserAsync(query, cancellationToken);
    }
}

public class ListAvailableTasksForUserQuery : IRequest<ListAvailableTasksForUser>
{
    public Guid UserId { get; set; }
    public int Page { get; set; }
}

public class ListAvailableTasksForUser
{
    public List<ListAvailableTasksForUserItem> Items { get; set; } = new List<ListAvailableTasksForUserItem>();
    public int TotalPages { get; set; }
}

public class ListAvailableTasksForUserItem
{
    public Guid Id { get; set; }
    public int Difficulty { get; set; }
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
}
