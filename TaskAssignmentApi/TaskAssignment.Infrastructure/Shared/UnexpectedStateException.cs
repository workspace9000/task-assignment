namespace TaskAssignment.Infrastructure.Shared
{
    public class UnexpectedStateException : Exception
    {
        public UnexpectedStateException() : base() { }
        public UnexpectedStateException(string message) : base(message) { }

        public UnexpectedStateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
