namespace Common.Tests.TestUtilities.DateTime;

/// <summary>
/// Test implementation of <see cref="IDateTimeProvider"/> that allows time freezing and manual advancement.
/// </summary>
public class DateTimeStub : IDateTimeProvider
{
    /// <summary>
    /// The frozen time used as the source for Now and UtcNow.
    /// </summary>
    public System.DateTime FrozenTime { get; private set; } = System.DateTime.UtcNow;

    /// <inheritdoc />
    public System.DateTime UtcNow => FrozenTime;

    /// <inheritdoc />
    public System.DateTime Now => FrozenTime.ToLocalTime();

    /// <summary>
    /// Freezes time to a specific UTC timestamp.
    /// </summary>
    /// <param name="timestamp">The UTC time to freeze to.</param>
    public void Freeze(System.DateTime timestamp)
    {
        FrozenTime = timestamp;
    }

    /// <summary>
    /// Advances the frozen time forward by a specified time span.
    /// </summary>
    /// <param name="duration">The amount of time to add to the frozen clock.</param>
    public void Advance(System.TimeSpan duration)
    {
        FrozenTime = FrozenTime.Add(duration);
    }
}