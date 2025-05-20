using System.Transactions;
using TaskAssignment.Application.Interfaces;
using TaskAssignment.Application.Interfaces.Settings;

namespace TaskAssignment.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseSettings _databaseSettings;
        private readonly TaskAssignmentDbContext _dbContext;

        public UnitOfWork(DatabaseSettings databaseSettings, TaskAssignmentDbContext dbContext)
        {
            _databaseSettings = databaseSettings;
            _dbContext = dbContext;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task CommitCurrentTransactionAsync()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.CurrentTransaction.CommitAsync();
            }
        }

        /// <summary>
        /// Creates async safe transaction scope with specified timeout
        /// </summary>
        /// <returns></returns>
        public TransactionScope CreateTransactionScope(int? minutesTimeout = null, TransactionScopeOption option = TransactionScopeOption.Required)
        {
            return new TransactionScope(option,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = minutesTimeout.HasValue && minutesTimeout.Value > 0 ? TimeSpan.FromMinutes(minutesTimeout.Value) : _databaseSettings.TransactionTimeout },
                TransactionScopeAsyncFlowOption.Enabled
                );
        }
    }
}
