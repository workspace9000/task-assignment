using FluentValidation;
using MediatR;
using TaskAssignment.Domain.Assignments;
using TaskAssignment.Domain.Exceptions;
using TaskAssignment.Domain.Tasks;
using TaskAssignment.Domain.Users;

namespace TaskAssignment.Application.Assignments;

public class AssignTasksCommandHandler : IRequestHandler<AssignTasksCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IAssignmentRepository _assignmentRepository;

    public AssignTasksCommandHandler(IUserRepository userRepository, ITaskRepository taskRepository, IAssignmentRepository assignmentRepository)
    {
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _assignmentRepository = assignmentRepository;
    }

    public async Task<Unit> Handle(AssignTasksCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                   ?? throw new ArgumentException("User not found");

        var allTasks = await _taskRepository.GetAllAsync(cancellationToken);
        var taskDict = allTasks.ToDictionary(t => t.Id);

        var newAssignments = request.TaskIds
            .Distinct()
            .Select(id =>
            {
                if (!taskDict.TryGetValue(id, out var task))
                    throw new ArgumentException($"Task {id} not found");

                return task;
            })
            .ToList();

        // R1: Zadanie może być przypisane tylko do jednego użytkownika
        var conflictingAssignments = await _assignmentRepository.AnyTaskAssignedToOtherUserAsync(taskIds: request.TaskIds, userId: request.UserId, cancellationToken);
        if (conflictingAssignments)
            throw new UserException("Some tasks are already assigned to other users");

        // R2: Min 5, max 11
        if (newAssignments.Count < 5 || newAssignments.Count > 11)
            throw new UserException("User must be assigned between 5 and 11 tasks");

        // R3 + R4: Ograniczenie typów zadań wg roli
        if (user.Role == UserRoles.Developer &&
            newAssignments.Any(t => t.Type != TaskTypes.Implementation))
        {
            throw new UserException("Developer can only have Implementacja tasks");
        }

        // R5–R7: Walidacja proporcji wg trudności
        int total = newAssignments.Count;
        int count45 = newAssignments.Count(t => t.Difficulty >= 4);
        int count12 = newAssignments.Count(t => t.Difficulty <= 2);
        int count3 = newAssignments.Count(t => t.Difficulty == 3);

        double pct45 = (double)count45 / total * 100;
        double pct12 = (double)count12 / total * 100;
        double pct3 = (double)count3 / total * 100;

        if (pct45 < 10 || pct45 > 30)
            throw new UserException("Tasks with difficulty 4–5 must be 10–30%");
        if (pct12 > 50)
            throw new UserException("Tasks with difficulty 1–2 must be <=50%");
        if (pct3 > 90)
            throw new UserException("Tasks with difficulty 3 must be <=90%");

        user.ReplaceAssignments(newAssignments.Select(t => t.Id));

        return Unit.Value;
    }
}

public class AssignTasksCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public List<Guid> TaskIds { get; set; } = new();
}

public class AssignTasksCommandValidator : AbstractValidator<AssignTasksCommand>
{
    public AssignTasksCommandValidator()
    {
    }
}