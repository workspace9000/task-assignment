namespace TaskAssignment.Application.Assignments
{
    public interface IUserTaskAssigmentsQueryRepository
    {
        Task<List<ListAssignedTasksForUserItem>> GetAssignedTasksForUserAsync(ListAssignedTasksForUserQuery query, CancellationToken cancellationToken);
    }
}
