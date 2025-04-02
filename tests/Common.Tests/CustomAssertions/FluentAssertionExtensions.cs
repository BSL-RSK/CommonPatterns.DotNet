using FluentAssertions;

namespace Common.Tests.CustomAssertions;

/// <summary>
/// Extension methods to simplify and clarify assertions using FluentAssertions.
/// These methods improve readability when working with common test scenarios.
/// </summary>
public static class FluentAssertionExtensions
{
    /// <summary>
    /// Asserts that a string contains all of the expected substrings.
    /// </summary>
    public static void ShouldContainAll(this string actual, params string[] expectedSubstrings)
    {
        foreach (var expected in expectedSubstrings)
        {
            actual.Should().Contain(expected);
        }
    }

    /// <summary>
    /// Asserts that a collection is not null or empty.
    /// </summary>
    public static void ShouldNotBeNullOrEmpty<T>(this IEnumerable<T>? collection)
    {
        collection.Should().NotBeNull();
        collection.Should().NotBeEmpty();
    }

    /// <summary>
    /// Asserts that an object is of a specific type and casts it for further assertions.
    /// </summary>
    public static TExpected ShouldBeOfType<TExpected>(this object actual)
    {
        actual.Should().BeOfType<TExpected>();
        return (TExpected)actual;
    }

    /// <summary>
    /// Asserts that two collections are equivalent regardless of order.
    /// </summary>
    public static void ShouldBeEquivalentToIgnoringOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
    {
        actual.Should().BeEquivalentTo(expected, options => options.WithoutStrictOrdering());
    }

    /// <summary>
    /// Asserts that the string contains at least one of the specified substrings.
    /// </summary>
    public static void ShouldContainAny(this string actual, params string[] options)
    {
        actual.Should().Match(x => options.Any(x.Contains),
            $"Expected string to contain at least one of: [{string.Join(", ", options)}]");
    }

    /// <summary>
    /// Asserts that a collection contains at least one of the expected values.
    /// </summary>
    public static void ShouldContainAny<T>(this IEnumerable<T> actual, params T[] expectedValues)
    {
        actual.Should().Contain(x => expectedValues.Contains(x));
    }

    /// <summary>
    /// Asserts that a collection does not contain any of the disallowed values.
    /// </summary>
    public static void ShouldNotContainAny<T>(this IEnumerable<T> actual, params T[] disallowedValues)
    {
        actual.Should().OnlyContain(x => !disallowedValues.Contains(x));
    }

    /// <summary>
    /// Asserts that the collection contains exactly one element and returns it.
    /// </summary>
    public static T ShouldHaveSingle<T>(this IEnumerable<T> actual)
    {
        actual.Should().ContainSingle();
        return actual.Single();
    }
}