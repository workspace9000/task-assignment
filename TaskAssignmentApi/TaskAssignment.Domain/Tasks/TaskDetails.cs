namespace TaskAssignment.Domain.Tasks;

public abstract class TaskDetails
{
    public Guid TaskId { get; protected set; }

    protected TaskDetails()
    {
        TaskId = Guid.NewGuid();
    }
}
