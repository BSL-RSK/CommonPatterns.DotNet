namespace Common.Tests.TestData;

/// <summary>
/// Provides shared data sets for use with xUnit's [MemberData] attribute.
/// Useful for reducing duplication in data-driven unit tests.
/// </summary>
public static class MemberDataProviders
{
    /// <summary>
    /// Simple string inputs and expected boolean result.
    /// </summary>
    public static IEnumerable<object[]> BasicValidationCases =>
        new List<object[]>
        {
            new object[] { "abc", true },
            new object[] { "", false },
            new object[] { null!, false },
            new object[] { "12345", true }
        };

    /// <summary>
    /// Complex object scenario for transformation or comparison tests.
    /// </summary>
    public static IEnumerable<object[]> UserRecords =>
        new List<object[]>
        {
            new object[]
            {
                new TestUser
                {
                    Id = "u1",
                    Name = "Alice",
                    Roles = new[] { "Admin" },
                    IsActive = true
                },
                true // expected isActive flag check
            },
            new object[]
            {
                new TestUser
                {
                    Id = "u2",
                    Name = "Bob",
                    Roles = new[] { "Guest" },
                    IsActive = false
                },
                false
            }
        };
}

/// <summary>
/// Minimal test user model for data-driven test scenarios.
/// </summary>
public class TestUser
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string[] Roles { get; set; } = Array.Empty<string>();
    public bool IsActive { get; set; }
}