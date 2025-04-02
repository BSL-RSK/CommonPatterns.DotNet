using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A comprehensive unit test template for a repository class with dependency injection.
/// 
/// ✅ Includes:
/// - Dependency validation
/// - Empty/null result handling
/// - [Theory] for input filtering or parameters
/// - [MemberData] for reusable test input sets
/// - Dependency call verification
/// - Logging assertion with ILogger
/// 
/// 🔁 What to replace:
/// - Rename this class to match the repository being tested
/// - Replace <c>SomeRepository</c>, <c>ISomeConnectionFactory</c>, <c>GetData</c> with your actual implementations
/// </summary>
public class GenericRepositoryTests
{
    #region Fields & Constructor

    private readonly Mock<ISomeConnectionFactory> _mockConnectionFactory;
    private readonly Mock<ILogger<SomeRepository>> _mockLogger;
    private readonly SomeRepository _sut;

    /// <summary>
    /// Initializes test class with mocked dependencies.
    /// </summary>
    public GenericRepositoryTests()
    {
        _mockConnectionFactory = new Mock<ISomeConnectionFactory>();
        _mockLogger = new Mock<ILogger<SomeRepository>>();
        _sut = new SomeRepository(_mockConnectionFactory.Object, _mockLogger.Object);
    }

    #endregion

    #region Constructor & Basic Behavior

    /// <summary>
    /// Ensures constructor throws if connection factory is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenConnectionFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SomeRepository(null, _mockLogger.Object));
    }

    /// <summary>
    /// Ensures valid data is returned from the repository.
    /// </summary>
    [Fact]
    public void GetData_ShouldReturnExpectedResults_WhenValid()
    {
        var expected = new List<string> { "one", "two" };
        _mockConnectionFactory.Setup(x => x.QueryData()).Returns(expected);

        var result = _sut.GetData();

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Ensures an empty list is returned when the data source is null.
    /// </summary>
    [Fact]
    public void GetData_ShouldReturnEmpty_WhenSourceReturnsNull()
    {
        _mockConnectionFactory.Setup(x => x.QueryData()).Returns((IEnumerable<string>)null!);

        var result = _sut.GetData();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    /// Ensures an exception is thrown when the underlying data source fails.
    /// </summary>
    [Fact]
    public void GetData_ShouldHandleDataException()
    {
        _mockConnectionFactory.Setup(x => x.QueryData()).Throws<InvalidOperationException>();

        var ex = Assert.Throws<RepositoryException>(() => _sut.GetData());
        Assert.Contains("failed to retrieve", ex.Message.ToLower());
    }

    /// <summary>
    /// Ensures that information is logged when data is successfully retrieved.
    /// </summary>
    [Fact]
    public void GetData_ShouldLogInfo_WhenDataRetrieved()
    {
        _mockConnectionFactory.Setup(x => x.QueryData()).Returns(new List<string> { "one" });

        _sut.GetData();

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Data retrieved")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Theory Tests

    /// <summary>
    /// Verifies the repository filters or processes results correctly for known inputs using InlineData.
    /// </summary>
    /// <param name="input">The input value to check for.</param>
    /// <param name="expected">Expected presence in the data (true/false).</param>
    [Theory]
    [InlineData("one", true)]
    [InlineData("unknown", false)]
    public void ContainsData_ShouldReturnCorrectResult_WhenCalled(string input, bool expected)
    {
        var items = new List<string> { "one", "two" };
        _mockConnectionFactory.Setup(x => x.QueryData()).Returns(items);

        var result = _sut.ContainsData(input);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that multiple data inputs produce expected results using shared test data.
    /// 🔗 Uses <see cref="RepositoryTestData.FilterInputCases"/> via [MemberData].
    /// </summary>
    /// <param name="input">Filter string passed to the repository.</param>
    /// <param name="expected">Expected filtered result list.</param>
    [Theory]
    [MemberData(nameof(RepositoryTestData.FilterInputCases), MemberType = typeof(RepositoryTestData))]
    public void FilterData_ShouldReturnExpectedSubset(string input, List<string> expected)
    {
        var items = new List<string> { "apple", "banana", "apricot", "cherry" };
        _mockConnectionFactory.Setup(x => x.QueryData()).Returns(items);

        var result = _sut.FilterData(input);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies repository handles invalid inputs gracefully using [MemberData].
    /// 🔗 Uses <see cref="RepositoryTestData.InvalidFilterInputs"/>.
    /// </summary>
    /// <param name="input">Invalid input string passed to the repository.</param>
    [Theory]
    [MemberData(nameof(RepositoryTestData.InvalidFilterInputs), MemberType = typeof(RepositoryTestData))]
    public void FilterData_ShouldReturnEmpty_WhenInputIsInvalid(string input)
    {
        var items = new List<string> { "apple", "banana", "apricot", "cherry" };
        _mockConnectionFactory.Setup(x => x.QueryData()).Returns(items);

        var result = _sut.FilterData(input);

        Assert.Empty(result);
    }

    #endregion
}

/// <summary>
/// Provides member data for repository tests.
/// </summary>
public static class RepositoryTestData
{
    /// <summary>
    /// Provides input/output cases for valid filter scenarios.
    /// </summary>
    public static IEnumerable<object[]> FilterInputCases =>
        new List<object[]>
        {
            new object[] { "a", new List<string> { "apple", "banana", "apricot" } },
            new object[] { "b", new List<string> { "banana" } },
            new object[] { "z", new List<string>() }
        };

    /// <summary>
    /// Provides various invalid input strings for testing empty result behavior.
    /// </summary>
    public static IEnumerable<object[]> InvalidFilterInputs =>
        new List<object[]>
        {
            new object[] { "" },
            new object[] { null! },
            new object[] { "     " }
        };
}
