using MediatR;

namespace TaskAssignment.Application.Assignments;

public class ListAssignedTasksForUserQueryHandler : IRequestHandler<ListAssignedTasksForUserQuery, List<ListAssignedTasksForUserItem>>
{
    private readonly IUserTaskAssigmentsQueryRepository _userTaskAssigmentsQueryRepository;

    public ListAssignedTasksForUserQueryHandler(IUserTaskAssigmentsQueryRepository userTaskAssigmentsQueryRepository)
    {
        _userTaskAssigmentsQueryRepository = userTaskAssigmentsQueryRepository;
    }

    public Task<List<ListAssignedTasksForUserItem>> Handle(ListAssignedTasksForUserQuery query, CancellationToken cancellationToken)
    {
        return _userTaskAssigmentsQueryRepository.GetAssignedTasksForUserAsync(query, cancellationToken);
    }
}

public class ListAssignedTasksForUserQuery : IRequest<List<ListAssignedTasksForUserItem>>
{
    public Guid UserId { get; set; }
    public int Page { get; set; }
}

public class ListAssignedTasksForUserItem
{
    public Guid Id { get; set; }
    public int Difficulty { get; set; }
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
}
