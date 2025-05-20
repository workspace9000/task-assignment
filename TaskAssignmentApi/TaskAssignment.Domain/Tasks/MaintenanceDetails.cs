namespace TaskAssignment.Domain.Tasks;

public class MaintenanceDetails : TaskDetails
{
    public DateTime Deadline { get; private set; }
    public string Services { get; private set; }
    public string Servers { get; private set; }

    public MaintenanceDetails(DateTime deadline, string services, string servers)
    {
        Deadline = deadline;
        Services = services;
        Servers = servers;
    }

    protected MaintenanceDetails() { }
}
