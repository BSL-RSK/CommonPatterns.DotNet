namespace Common.Tests.TestUtilities.DateTime;

/// <summary>
/// Abstraction over the system clock to enable time-based logic testing.
/// Use this interface in application code to avoid directly depending on <c>DateTime.UtcNow</c> or <c>DateTime.Now</c>.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC time.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current local time.
    /// </summary>
    DateTime Now { get; }
}