namespace TaskAssignment.Domain.Tasks;

public class ImplementationDetails : TaskDetails
{
    public string Content { get; private set; }

    public ImplementationDetails(string content)
    {
        Content = content;
    }

    protected ImplementationDetails() { }
}
