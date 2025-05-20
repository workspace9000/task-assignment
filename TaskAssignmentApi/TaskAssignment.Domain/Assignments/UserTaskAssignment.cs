namespace TaskAssignment.Domain.Assignments;

public class UserTaskAssignment
{
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; }

    public UserTaskAssignment(Guid taskId, Guid userId)
    {
        TaskId = taskId;
        UserId = userId;
    }

    protected UserTaskAssignment() { }
}
