namespace Common.Tests.TestUtilities.DateTime;

/// <summary>
/// Default implementation of <see cref="IDateTimeProvider"/> that returns actual system time.
/// Used in production to provide current UTC and local time.
/// </summary>
public class SystemDateTimeProvider : IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC time from <see cref="System.DateTime.UtcNow"/>.
    /// </summary>
    public DateTime UtcNow => DateTime.UtcNow;

    /// <summary>
    /// Gets the current local time from <see cref="System.DateTime.Now"/>.
    /// </summary>
    public DateTime Now => DateTime.Now;
}