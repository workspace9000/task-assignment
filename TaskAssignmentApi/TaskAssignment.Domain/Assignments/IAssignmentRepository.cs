namespace TaskAssignment.Domain.Assignments
{
    public interface IAssignmentRepository
    {
        Task<bool> AnyTaskAssignedToOtherUserAsync(IEnumerable<Guid> taskIds, Guid userId, CancellationToken cancellationToken);
    }
}
