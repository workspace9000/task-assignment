namespace TaskAssignment.Infrastructure.Shared
{
    /// <summary>
    /// Reassigning (SetDateTime) allowed in unit tests mode only! For tests based on current date this provider helps taking control over setting current date for Now.
    /// Running unit tests in parallel mode not recommended for logic using the provider.
    /// </summary>
    public static class DateTimeProvider
    {
        private static Func<DateTime> _now = () => DateTime.Now;
        public static DateTime Now => _now();

#if DEBUG
        public static void SetDateTime(DateTime dateTime)
        {
            _now = () => dateTime;
        }

        public static void ResetToDefault()
        {
            _now = () => DateTime.Now;
        }
#endif
    }

}
