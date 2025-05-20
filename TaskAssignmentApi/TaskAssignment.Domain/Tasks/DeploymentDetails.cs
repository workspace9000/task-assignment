namespace TaskAssignment.Domain.Tasks;

public class DeploymentDetails : TaskDetails
{
    public DateTime Deadline { get; private set; }
    public string Scope { get; private set; }

    public DeploymentDetails(DateTime deadline, string scope)
    {
        Deadline = deadline;
        Scope = scope;
    }

    protected DeploymentDetails() { }
}
