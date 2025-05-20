using TaskAssignment.Application.Interfaces;

namespace TaskAssignment.Infrastructure.Shared
{
    public class HandlerContext : IHandlerContext
    {
        private bool _dontUseCommandValidator;
        public bool IsDontUseCommandValidator => _dontUseCommandValidator;

        public void DontUseCommandValidator()
        {
            _dontUseCommandValidator = true;
        }
    }
}
