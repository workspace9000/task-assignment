using Microsoft.EntityFrameworkCore;
using TaskAssignment.Application.Interfaces;
using TaskAssignment.Application.Interfaces.Settings;

namespace TaskAssignment.Infrastructure.Database
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DatabaseSettings _databaseSettings;
        private readonly DbContextOptions<TaskAssignmentDbContext> _dbTaskAssignmentOptions;

        public UnitOfWorkFactory(DatabaseSettings databaseSettings, DbContextOptions<TaskAssignmentDbContext> dbTaskAssignmentOptions)
        {
            _databaseSettings = databaseSettings;
            _dbTaskAssignmentOptions = dbTaskAssignmentOptions;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_databaseSettings, new TaskAssignmentDbContext(_dbTaskAssignmentOptions));
        }
    }
}
