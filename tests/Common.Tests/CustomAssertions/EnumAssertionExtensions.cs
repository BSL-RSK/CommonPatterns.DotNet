namespace Common.Tests.CustomAssertions;

/// <summary>
/// Extension methods for asserting enum validity and parsing expectations.
/// </summary>
public static class EnumAssertionExtensions
{
    /// <summary>
    /// Asserts that a string can be parsed into a defined value of the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to test against.</typeparam>
    /// <param name="actual">The input string to parse.</param>
    public static void ShouldBeDefinedEnum<TEnum>(this string actual) where TEnum : struct, Enum
    {
        var isValid = Enum.TryParse<TEnum>(actual, ignoreCase: true, out var parsed)
                      && Enum.IsDefined(typeof(TEnum), parsed);

        isValid.Should().BeTrue($"'{actual}' should be a valid value of enum {typeof(TEnum).Name}");
    }

    /// <summary>
    /// Asserts that an enum value is defined in its enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The enum value to check.</param>
    public static void ShouldBeValidEnumValue<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        Enum.IsDefined(typeof(TEnum), value).Should().BeTrue($"'{value}' is not a valid value of enum {typeof(TEnum).Name}");
    }
}