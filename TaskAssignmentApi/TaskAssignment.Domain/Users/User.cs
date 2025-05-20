using TaskAssignment.Domain.Assignments;

namespace TaskAssignment.Domain.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public UserRoles Role { get; private set; }

    public ICollection<UserTaskAssignment> Assignments { get; private set; } = new List<UserTaskAssignment>();

    protected User() { }

    public User(string name, UserRoles role)
    {
        Id = Guid.NewGuid();
        Name = name;
        Role = role;
    }

    public void ReplaceAssignments(IEnumerable<Guid> newTaskIds)
    {
        Assignments.Clear();
        foreach (var taskId in newTaskIds)
        {
            Assignments.Add(new UserTaskAssignment(Id, taskId));
        }
    }
}
