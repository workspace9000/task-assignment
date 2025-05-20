using System.Transactions;

namespace TaskAssignment.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task CommitCurrentTransactionAsync();
        TransactionScope CreateTransactionScope(int? minutesTimeout = null, TransactionScopeOption option = TransactionScopeOption.Required);
    }
}