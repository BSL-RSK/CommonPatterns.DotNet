using FluentAssertions;

namespace Common.Tests.CustomAssertions;

/// <summary>
/// Extension methods for asserting object comparisons and state with flexibility and readability.
/// Useful for complex object graphs and partial assertions.
/// </summary>
public static class ObjectAssertionExtensions
{
    /// <summary>
    /// Asserts that two objects are equivalent, ignoring specific properties.
    /// </summary>
    /// <typeparam name="T">The object type being compared.</typeparam>
    /// <param name="actual">The actual object under test.</param>
    /// <param name="expected">The expected object to match.</param>
    /// <param name="excludedProperties">List of properties to ignore in comparison.</param>
    public static void ShouldBeEquivalentToIgnoring<T>(this T actual, T expected, params string[] excludedProperties)
    {
        actual.Should().BeEquivalentTo(expected, options =>
            options.Excluding(ctx => excludedProperties.Any(p => ctx.Path.Equals(p))));
    }

    /// <summary>
    /// Asserts that the object has default values for all properties.
    /// </summary>
    /// <typeparam name="T">The object type being inspected.</typeparam>
    /// <param name="actual">The object instance.</param>
    public static void ShouldHaveDefaultValues<T>(this T actual)
    {
        var defaultObj = Activator.CreateInstance<T>();
        actual.Should().BeEquivalentTo(defaultObj);
    }

    /// <summary>
    /// Asserts that the object is deeply equivalent to another but using strict ordering on collections.
    /// </summary>
    /// <typeparam name="T">Type of the objects being compared.</typeparam>
    /// <param name="actual">The object under test.</param>
    /// <param name="expected">The expected equivalent object.</param>
    public static void ShouldBeExactlyEquivalentTo<T>(this T actual, T expected)
    {
        actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    /// <summary>
    /// Asserts that the actual object contains a subset of properties from the expected object.
    /// </summary>
    /// <typeparam name="T">The type of object being asserted.</typeparam>
    /// <param name="actual">The full object under test.</param>
    /// <param name="expectedSubset">The expected partial structure.</param>
    public static void ShouldContainPartial<T>(this T actual, object expectedSubset)
    {
        actual.Should().BeEquivalentTo(expectedSubset, options => options.ExcludingMissingMembers());
    }

    /// <summary>
    /// Asserts that an object's specified property has changed value from its original.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="original">Original version of the object.</param>
    /// <param name="modified">Modified version to compare.</param>
    /// <param name="propertyName">The property that should differ between the two.</param>
    public static void ShouldHaveChanged<T>(this T original, T modified, string propertyName)
    {
        var originalValue = typeof(T).GetProperty(propertyName)?.GetValue(original);
        var modifiedValue = typeof(T).GetProperty(propertyName)?.GetValue(modified);

        originalValue.Should().NotBeEquivalentTo(modifiedValue, $"Property '{propertyName}' was expected to change.");
    }
}