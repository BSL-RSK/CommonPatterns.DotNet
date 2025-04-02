using FluentAssertions;

namespace Common.Tests.CustomAssertions;

/// <summary>
/// Extension methods to simplify common assertions on <see cref="DateTime"/> values.
/// Useful for testing time-sensitive behavior with readable syntax.
/// </summary>
public static class DateTimeAssertionExtensions
{
    /// <summary>
    /// Asserts that the actual time is within a given number of seconds of the expected time.
    /// </summary>
    /// <param name="actual">The actual DateTime value.</param>
    /// <param name="expected">The expected DateTime value.</param>
    /// <param name="seconds">The number of seconds tolerance. Default is 5.</param>
    public static void ShouldBeCloseTo(this DateTime actual, DateTime expected, int seconds = 5)
    {
        actual.Should().BeCloseTo(expected, TimeSpan.FromSeconds(seconds));
    }

    /// <summary>
    /// Asserts that the DateTime value is after the specified threshold.
    /// </summary>
    public static void ShouldBeAfter(this DateTime actual, DateTime threshold)
    {
        actual.Should().BeAfter(threshold);
    }

    /// <summary>
    /// Asserts that the DateTime value is before the specified threshold.
    /// </summary>
    public static void ShouldBeBefore(this DateTime actual, DateTime threshold)
    {
        actual.Should().BeBefore(threshold);
    }

    /// <summary>
    /// Asserts that the DateTime is between two values inclusively.
    /// </summary>
    public static void ShouldBeBetweenInclusive(this DateTime actual, DateTime from, DateTime to)
    {
        actual.Should().BeOnOrAfter(from).And.BeOnOrBefore(to);
    }
}