namespace TaskAssignment.Application.Interfaces
{
    public interface IHandlerContext
    {
        /// <summary>
        /// Validation can be turned off with DontUseCommandValidator method. (Default value: false).
        /// </summary>
        bool IsDontUseCommandValidator { get; }
        void DontUseCommandValidator();
    }
}
