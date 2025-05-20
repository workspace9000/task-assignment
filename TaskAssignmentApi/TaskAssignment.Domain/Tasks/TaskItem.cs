namespace TaskAssignment.Domain.Tasks;

public class TaskItem
{
    public Guid Id { get; private set; }
    public int Difficulty { get; private set; }
    public TaskTypes Type { get; private set; }
    public TaskStatuses Status { get; private set; }

    private TaskItem() { }

    public TaskItem(int difficulty, TaskTypes type, TaskStatuses status)
    {
        Id = Guid.NewGuid();
        Difficulty = difficulty;
        Type = type;
        Status = status;
    }

    public void UpdateStatus(TaskStatuses newStatus) => Status = newStatus;
}
