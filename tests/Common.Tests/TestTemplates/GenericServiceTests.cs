using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A comprehensive unit test template for a service class with dependency injection.
///
/// ✅ Includes:
/// - Constructor setup for shared dependencies
/// - Edge case coverage
/// - Input validation and fallback behavior
/// - Dependency call verification
/// - Logging assertion with <c>ILogger&lt;T&gt;</c>
///
/// 🔁 What to replace:
/// - Rename this class to match the service being tested
/// - Replace <c>SomeService</c>, <c>ISomeDependency</c>, and <c>DoSomething</c> with your actual names
/// </summary>
public class GenericServiceTests
{using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A comprehensive unit test template for a service class with dependency injection.
///
/// ✅ Includes:
/// - Constructor setup for shared dependencies
/// - Edge case coverage
/// - Input validation and fallback behavior
/// - Dependency call verification
/// - Logging assertion with <c>ILogger&lt;T&gt;</c>
///
/// 🔁 What to replace:
/// - Rename this class to match the service being tested
/// - Replace <c>SomeService</c>, <c>ISomeDependency</c>, and <c>DoSomething</c> with your actual names
/// </summary>
public class GenericServiceTests
{
    #region Fields & Constructor

    private readonly Mock<ISomeDependency> _mockDependency;
    private readonly Mock<ILogger<SomeService>> _mockLogger;
    private readonly SomeService _sut;

    /// <summary>
    /// Sets up shared mocks and initializes the System Under Test (SUT).
    /// </summary>
    public GenericServiceTests()
    {
        _mockDependency = new Mock<ISomeDependency>();
        _mockLogger = new Mock<ILogger<SomeService>>();
        _sut = new SomeService(_mockDependency.Object, _mockLogger.Object);
    }

    #endregion

    #region Constructor & Input Validation

    [Fact]
    public void Constructor_ShouldThrow_WhenDependencyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SomeService(null, _mockLogger.Object));
    }

    [Fact]
    public void Method_ShouldThrow_WhenInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.DoSomething(null));
    }

    [Fact]
    public void Method_ShouldNotCallDependency_WhenInputIsInvalid()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.DoSomething(""));
        _mockDependency.Verify(x => x.DoWork(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Happy Path & Output Verification

    [Fact]
    public void Method_ShouldReturnExpectedResult_WhenValidInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("expected");

        var result = _sut.DoSomething("input");

        Assert.Equal("expected", result);
    }

    [Fact]
    public void Method_ShouldReturnEmpty_WhenDependencyReturnsNull()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns((string)null);

        var result = _sut.DoSomething("input");

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Method_ShouldTrimInput_WhenWhitespaceProvided()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var result = _sut.DoSomething("  input  ");

        Assert.Equal("result", result);
    }

    [Fact]
    public void Method_ShouldBeCaseInsensitive_WhenConfigured()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var resultLower = _sut.DoSomething("input");
        var resultUpper = _sut.DoSomething("INPUT");

        Assert.Equal(resultLower, resultUpper);
    }

    [Fact]
    public void Method_ShouldSupportUnicodeCharacters()
    {
        _mockDependency.Setup(x => x.DoWork("café")).Returns("processed");

        var result = _sut.DoSomething("café");

        Assert.Equal("processed", result);
    }

    #endregion

    #region Dependency Behavior Verification

    [Fact]
    public void Method_ShouldCallDependencyOnce_WithExpectedInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("done");

        _sut.DoSomething("input");

        _mockDependency.Verify(x => x.DoWork("input"), Times.Once);
    }

    [Fact]
    public void Method_ShouldHandleUnexpectedDependencyException()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Throws<InvalidOperationException>();

        var ex = Assert.Throws<ServiceOperationException>(() => _sut.DoSomething("input"));
        Assert.Contains("unexpected error", ex.Message.ToLower());
    }

    #endregion

    #region Logging

    [Fact]
    public void Method_ShouldLogInformation_WhenCalled()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        _sut.DoSomething("input");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Called DoSomething")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Theory Tests (InlineData & MemberData)

    /// <summary>
    /// Verifies that the service returns expected results for a variety of valid inputs using InlineData.
    /// </summary>
    [Theory]
    [InlineData("input", "expected")]
    [InlineData("  input  ", "expected")]
    [InlineData("INPUT", "expected")]
    [InlineData("café", "processed")]
    public void Method_ShouldReturnExpected_ForValidInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that the service transforms multiple inputs into the expected results using MemberData.
    /// </summary>
    [Theory]
    [MemberData(nameof(ServiceTestData.ComplexInputCases), MemberType = typeof(ServiceTestData))]
    public void Method_ShouldReturnExpected_ForMemberDataInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(input)).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    #endregion
}

/// <summary>
/// Provides reusable test data for [MemberData] scenarios.
/// </summary>
public static class ServiceTestData
{
    public static IEnumerable<object[]> ComplexInputCases =>
        new List<object[]>
        {
            new object[] { "a", "resultA" },
            new object[] { "b", "resultB" },
            new object[] { "C", "resultC" }
        };
}
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A comprehensive unit test template for a service class with dependency injection.
///
/// ✅ Includes:
/// - Constructor setup for shared dependencies
/// - Edge case coverage
/// - Input validation and fallback behavior
/// - Dependency call verification
/// - Logging assertion with <c>ILogger&lt;T&gt;</c>
///
/// 🔁 What to replace:
/// - Rename this class to match the service being tested
/// - Replace <c>SomeService</c>, <c>ISomeDependency</c>, and <c>DoSomething</c> with your actual names
/// </summary>
public class GenericServiceTests
{
    #region Fields & Constructor

    private readonly Mock<ISomeDependency> _mockDependency;
    private readonly Mock<ILogger<SomeService>> _mockLogger;
    private readonly SomeService _sut;

    /// <summary>
    /// Sets up shared mocks and initializes the System Under Test (SUT).
    /// </summary>
    public GenericServiceTests()
    {
        _mockDependency = new Mock<ISomeDependency>();
        _mockLogger = new Mock<ILogger<SomeService>>();
        _sut = new SomeService(_mockDependency.Object, _mockLogger.Object);
    }

    #endregion

    #region Constructor & Input Validation

    [Fact]
    public void Constructor_ShouldThrow_WhenDependencyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SomeService(null, _mockLogger.Object));
    }

    [Fact]
    public void Method_ShouldThrow_WhenInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.DoSomething(null));
    }

    [Fact]
    public void Method_ShouldNotCallDependency_WhenInputIsInvalid()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.DoSomething(""));
        _mockDependency.Verify(x => x.DoWork(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Happy Path & Output Verification

    [Fact]
    public void Method_ShouldReturnExpectedResult_WhenValidInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("expected");

        var result = _sut.DoSomething("input");

        Assert.Equal("expected", result);
    }

    [Fact]
    public void Method_ShouldReturnEmpty_WhenDependencyReturnsNull()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns((string)null);

        var result = _sut.DoSomething("input");

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Method_ShouldTrimInput_WhenWhitespaceProvided()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var result = _sut.DoSomething("  input  ");

        Assert.Equal("result", result);
    }

    [Fact]
    public void Method_ShouldBeCaseInsensitive_WhenConfigured()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var resultLower = _sut.DoSomething("input");
        var resultUpper = _sut.DoSomething("INPUT");

        Assert.Equal(resultLower, resultUpper);
    }

    [Fact]
    public void Method_ShouldSupportUnicodeCharacters()
    {
        _mockDependency.Setup(x => x.DoWork("café")).Returns("processed");

        var result = _sut.DoSomething("café");

        Assert.Equal("processed", result);
    }

    #endregion

    #region Dependency Behavior Verification

    [Fact]
    public void Method_ShouldCallDependencyOnce_WithExpectedInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("done");

        _sut.DoSomething("input");

        _mockDependency.Verify(x => x.DoWork("input"), Times.Once);
    }

    [Fact]
    public void Method_ShouldHandleUnexpectedDependencyException()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Throws<InvalidOperationException>();

        var ex = Assert.Throws<ServiceOperationException>(() => _sut.DoSomething("input"));
        Assert.Contains("unexpected error", ex.Message.ToLower());
    }

    #endregion

    #region Logging

    [Fact]
    public void Method_ShouldLogInformation_WhenCalled()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        _sut.DoSomething("input");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Called DoSomething")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Theory Tests (InlineData & MemberData)

    /// <summary>
    /// Verifies that the service returns expected results for a variety of valid inputs using InlineData.
    /// </summary>
    [Theory]
    [InlineData("input", "expected")]
    [InlineData("  input  ", "expected")]
    [InlineData("INPUT", "expected")]
    [InlineData("café", "processed")]
    public void Method_ShouldReturnExpected_ForValidInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that the service transforms multiple inputs into the expected results using MemberData.
    /// </summary>
    [Theory]
    [MemberData(nameof(ServiceTestData.ComplexInputCases), MemberType = typeof(ServiceTestData))]
    public void Method_ShouldReturnExpected_ForMemberDataInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(input)).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    #endregion
}

/// <summary>
/// Provides reusable test data for [MemberData] scenarios.
/// </summary>
public static class ServiceTestData
{
    public static IEnumerable<object[]> ComplexInputCases =>
        new List<object[]>
        {
            new object[] { "a", "resultA" },
            new object[] { "b", "resultB" },
            new object[] { "C", "resultC" }
        };
}
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A comprehensive unit test template for a service class with dependency injection.
///
/// ✅ Includes:
/// - Constructor setup for shared dependencies
/// - Edge case coverage
/// - Input validation and fallback behavior
/// - Dependency call verification
/// - Logging assertion with <c>ILogger&lt;T&gt;</c>
///
/// 🔁 What to replace:
/// - Rename this class to match the service being tested
/// - Replace <c>SomeService</c>, <c>ISomeDependency</c>, and <c>DoSomething</c> with your actual names
/// </summary>
public class GenericServiceTests
{
    #region Fields & Constructor

    private readonly Mock<ISomeDependency> _mockDependency;
    private readonly Mock<ILogger<SomeService>> _mockLogger;
    private readonly SomeService _sut;

    /// <summary>
    /// Sets up shared mocks and initializes the System Under Test (SUT).
    /// </summary>
    public GenericServiceTests()
    {
        _mockDependency = new Mock<ISomeDependency>();
        _mockLogger = new Mock<ILogger<SomeService>>();
        _sut = new SomeService(_mockDependency.Object, _mockLogger.Object);
    }

    #endregion

    #region Constructor & Input Validation

    [Fact]
    public void Constructor_ShouldThrow_WhenDependencyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SomeService(null, _mockLogger.Object));
    }

    [Fact]
    public void Method_ShouldThrow_WhenInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.DoSomething(null));
    }

    [Fact]
    public void Method_ShouldNotCallDependency_WhenInputIsInvalid()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.DoSomething(""));
        _mockDependency.Verify(x => x.DoWork(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Happy Path & Output Verification

    [Fact]
    public void Method_ShouldReturnExpectedResult_WhenValidInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("expected");

        var result = _sut.DoSomething("input");

        Assert.Equal("expected", result);
    }

    [Fact]
    public void Method_ShouldReturnEmpty_WhenDependencyReturnsNull()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns((string)null);

        var result = _sut.DoSomething("input");

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Method_ShouldTrimInput_WhenWhitespaceProvided()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var result = _sut.DoSomething("  input  ");

        Assert.Equal("result", result);
    }

    [Fact]
    public void Method_ShouldBeCaseInsensitive_WhenConfigured()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var resultLower = _sut.DoSomething("input");
        var resultUpper = _sut.DoSomething("INPUT");

        Assert.Equal(resultLower, resultUpper);
    }

    [Fact]
    public void Method_ShouldSupportUnicodeCharacters()
    {
        _mockDependency.Setup(x => x.DoWork("café")).Returns("processed");

        var result = _sut.DoSomething("café");

        Assert.Equal("processed", result);
    }

    #endregion

    #region Dependency Behavior Verification

    [Fact]
    public void Method_ShouldCallDependencyOnce_WithExpectedInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("done");

        _sut.DoSomething("input");

        _mockDependency.Verify(x => x.DoWork("input"), Times.Once);
    }

    [Fact]
    public void Method_ShouldHandleUnexpectedDependencyException()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Throws<InvalidOperationException>();

        var ex = Assert.Throws<ServiceOperationException>(() => _sut.DoSomething("input"));
        Assert.Contains("unexpected error", ex.Message.ToLower());
    }

    #endregion

    #region Logging

    [Fact]
    public void Method_ShouldLogInformation_WhenCalled()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        _sut.DoSomething("input");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Called DoSomething")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Theory Tests (InlineData & MemberData)

    /// <summary>
    /// Verifies that the service returns expected results for a variety of valid inputs using InlineData.
    /// </summary>
    [Theory]
    [InlineData("input", "expected")]
    [InlineData("  input  ", "expected")]
    [InlineData("INPUT", "expected")]
    [InlineData("café", "processed")]
    public void Method_ShouldReturnExpected_ForValidInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that the service transforms multiple inputs into the expected results using MemberData.
    /// </summary>
    [Theory]
    [MemberData(nameof(ServiceTestData.ComplexInputCases), MemberType = typeof(ServiceTestData))]
    public void Method_ShouldReturnExpected_ForMemberDataInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(input)).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    #endregion
}

/// <summary>
/// Provides reusable test data for [MemberData] scenarios.
/// </summary>
public static class ServiceTestData
{
    public static IEnumerable<object[]> ComplexInputCases =>
        new List<object[]>
        {
            new object[] { "a", "resultA" },
            new object[] { "b", "resultB" },
            new object[] { "C", "resultC" }
        };
}
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A comprehensive unit test template for a service class with dependency injection.
///
/// ✅ Includes:
/// - Constructor setup for shared dependencies
/// - Edge case coverage
/// - Input validation and fallback behavior
/// - Dependency call verification
/// - Logging assertion with <c>ILogger&lt;T&gt;</c>
///
/// 🔁 What to replace:
/// - Rename this class to match the service being tested
/// - Replace <c>SomeService</c>, <c>ISomeDependency</c>, and <c>DoSomething</c> with your actual names
/// </summary>
public class GenericServiceTests
{
    #region Fields & Constructor

    private readonly Mock<ISomeDependency> _mockDependency;
    private readonly Mock<ILogger<SomeService>> _mockLogger;
    private readonly SomeService _sut;

    /// <summary>
    /// Sets up shared mocks and initializes the System Under Test (SUT).
    /// </summary>
    public GenericServiceTests()
    {
        _mockDependency = new Mock<ISomeDependency>();
        _mockLogger = new Mock<ILogger<SomeService>>();
        _sut = new SomeService(_mockDependency.Object, _mockLogger.Object);
    }

    #endregion

    #region Constructor & Input Validation

    [Fact]
    public void Constructor_ShouldThrow_WhenDependencyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SomeService(null, _mockLogger.Object));
    }

    [Fact]
    public void Method_ShouldThrow_WhenInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.DoSomething(null));
    }

    [Fact]
    public void Method_ShouldNotCallDependency_WhenInputIsInvalid()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.DoSomething(""));
        _mockDependency.Verify(x => x.DoWork(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Happy Path & Output Verification

    [Fact]
    public void Method_ShouldReturnExpectedResult_WhenValidInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("expected");

        var result = _sut.DoSomething("input");

        Assert.Equal("expected", result);
    }

    [Fact]
    public void Method_ShouldReturnEmpty_WhenDependencyReturnsNull()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns((string)null);

        var result = _sut.DoSomething("input");

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Method_ShouldTrimInput_WhenWhitespaceProvided()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var result = _sut.DoSomething("  input  ");

        Assert.Equal("result", result);
    }

    [Fact]
    public void Method_ShouldBeCaseInsensitive_WhenConfigured()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var resultLower = _sut.DoSomething("input");
        var resultUpper = _sut.DoSomething("INPUT");

        Assert.Equal(resultLower, resultUpper);
    }

    [Fact]
    public void Method_ShouldSupportUnicodeCharacters()
    {
        _mockDependency.Setup(x => x.DoWork("café")).Returns("processed");

        var result = _sut.DoSomething("café");

        Assert.Equal("processed", result);
    }

    #endregion

    #region Dependency Behavior Verification

    [Fact]
    public void Method_ShouldCallDependencyOnce_WithExpectedInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("done");

        _sut.DoSomething("input");

        _mockDependency.Verify(x => x.DoWork("input"), Times.Once);
    }

    [Fact]
    public void Method_ShouldHandleUnexpectedDependencyException()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Throws<InvalidOperationException>();

        var ex = Assert.Throws<ServiceOperationException>(() => _sut.DoSomething("input"));
        Assert.Contains("unexpected error", ex.Message.ToLower());
    }

    #endregion

    #region Logging

    [Fact]
    public void Method_ShouldLogInformation_WhenCalled()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        _sut.DoSomething("input");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Called DoSomething")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Theory Tests (InlineData & MemberData)

    /// <summary>
    /// Verifies that the service returns expected results for a variety of valid inputs using InlineData.
    /// </summary>
    [Theory]
    [InlineData("input", "expected")]
    [InlineData("  input  ", "expected")]
    [InlineData("INPUT", "expected")]
    [InlineData("café", "processed")]
    public void Method_ShouldReturnExpected_ForValidInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that the service transforms multiple inputs into the expected results using MemberData.
    /// </summary>
    [Theory]
    [MemberData(nameof(ServiceTestData.ComplexInputCases), MemberType = typeof(ServiceTestData))]
    public void Method_ShouldReturnExpected_ForMemberDataInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(input)).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    #endregion
}

/// <summary>
/// Provides reusable test data for [MemberData] scenarios.
/// </summary>
public static class ServiceTestData
{
    public static IEnumerable<object[]> ComplexInputCases =>
        new List<object[]>
        {
            new object[] { "a", "resultA" },
            new object[] { "b", "resultB" },
            new object[] { "C", "resultC" }
        };
}
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A comprehensive unit test template for a service class with dependency injection.
///
/// ✅ Includes:
/// - Constructor setup for shared dependencies
/// - Edge case coverage
/// - Input validation and fallback behavior
/// - Dependency call verification
/// - Logging assertion with <c>ILogger&lt;T&gt;</c>
///
/// 🔁 What to replace:
/// - Rename this class to match the service being tested
/// - Replace <c>SomeService</c>, <c>ISomeDependency</c>, and <c>DoSomething</c> with your actual names
/// </summary>
public class GenericServiceTests
{
    #region Fields & Constructor

    private readonly Mock<ISomeDependency> _mockDependency;
    private readonly Mock<ILogger<SomeService>> _mockLogger;
    private readonly SomeService _sut;

    /// <summary>
    /// Sets up shared mocks and initializes the System Under Test (SUT).
    /// </summary>
    public GenericServiceTests()
    {
        _mockDependency = new Mock<ISomeDependency>();
        _mockLogger = new Mock<ILogger<SomeService>>();
        _sut = new SomeService(_mockDependency.Object, _mockLogger.Object);
    }

    #endregion

    #region Constructor & Input Validation

    [Fact]
    public void Constructor_ShouldThrow_WhenDependencyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SomeService(null, _mockLogger.Object));
    }

    [Fact]
    public void Method_ShouldThrow_WhenInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.DoSomething(null));
    }

    [Fact]
    public void Method_ShouldNotCallDependency_WhenInputIsInvalid()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.DoSomething(""));
        _mockDependency.Verify(x => x.DoWork(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Happy Path & Output Verification

    [Fact]
    public void Method_ShouldReturnExpectedResult_WhenValidInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("expected");

        var result = _sut.DoSomething("input");

        Assert.Equal("expected", result);
    }

    [Fact]
    public void Method_ShouldReturnEmpty_WhenDependencyReturnsNull()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns((string)null);

        var result = _sut.DoSomething("input");

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Method_ShouldTrimInput_WhenWhitespaceProvided()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var result = _sut.DoSomething("  input  ");

        Assert.Equal("result", result);
    }

    [Fact]
    public void Method_ShouldBeCaseInsensitive_WhenConfigured()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var resultLower = _sut.DoSomething("input");
        var resultUpper = _sut.DoSomething("INPUT");

        Assert.Equal(resultLower, resultUpper);
    }

    [Fact]
    public void Method_ShouldSupportUnicodeCharacters()
    {
        _mockDependency.Setup(x => x.DoWork("café")).Returns("processed");

        var result = _sut.DoSomething("café");

        Assert.Equal("processed", result);
    }

    #endregion

    #region Dependency Behavior Verification

    [Fact]
    public void Method_ShouldCallDependencyOnce_WithExpectedInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("done");

        _sut.DoSomething("input");

        _mockDependency.Verify(x => x.DoWork("input"), Times.Once);
    }

    [Fact]
    public void Method_ShouldHandleUnexpectedDependencyException()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Throws<InvalidOperationException>();

        var ex = Assert.Throws<ServiceOperationException>(() => _sut.DoSomething("input"));
        Assert.Contains("unexpected error", ex.Message.ToLower());
    }

    #endregion

    #region Logging

    [Fact]
    public void Method_ShouldLogInformation_WhenCalled()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        _sut.DoSomething("input");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Called DoSomething")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Theory Tests (InlineData & MemberData)

    /// <summary>
    /// Verifies that the service returns expected results for a variety of valid inputs using InlineData.
    /// </summary>
    [Theory]
    [InlineData("input", "expected")]
    [InlineData("  input  ", "expected")]
    [InlineData("INPUT", "expected")]
    [InlineData("café", "processed")]
    public void Method_ShouldReturnExpected_ForValidInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that the service transforms multiple inputs into the expected results using MemberData.
    /// </summary>
    [Theory]
    [MemberData(nameof(ServiceTestData.ComplexInputCases), MemberType = typeof(ServiceTestData))]
    public void Method_ShouldReturnExpected_ForMemberDataInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(input)).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    #endregion
}

/// <summary>
/// Provides reusable test data for [MemberData] scenarios.
/// </summary>
public static class ServiceTestData
{
    public static IEnumerable<object[]> ComplexInputCases =>
        new List<object[]>
        {
            new object[] { "a", "resultA" },
            new object[] { "b", "resultB" },
            new object[] { "C", "resultC" }
        };
}

    #region Fields & Constructor

    private readonly Mock<ISomeDependency> _mockDependency;
    private readonly Mock<ILogger<SomeService>> _mockLogger;
    private readonly SomeService _sut;

    /// <summary>
    /// Sets up shared mocks and initializes the System Under Test (SUT).
    /// </summary>
    public GenericServiceTests()
    {
        _mockDependency = new Mock<ISomeDependency>();
        _mockLogger = new Mock<ILogger<SomeService>>();
        _sut = new SomeService(_mockDependency.Object, _mockLogger.Object);
    }

    #endregion

    #region Constructor & Input Validation

    [Fact]
    public void Constructor_ShouldThrow_WhenDependencyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SomeService(null, _mockLogger.Object));
    }

    [Fact]
    public void Method_ShouldThrow_WhenInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.DoSomething(null));
    }

    [Fact]
    public void Method_ShouldNotCallDependency_WhenInputIsInvalid()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.DoSomething(""));
        _mockDependency.Verify(x => x.DoWork(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Happy Path & Output Verification

    [Fact]
    public void Method_ShouldReturnExpectedResult_WhenValidInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("expected");

        var result = _sut.DoSomething("input");

        Assert.Equal("expected", result);
    }

    [Fact]
    public void Method_ShouldReturnEmpty_WhenDependencyReturnsNull()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns((string)null);

        var result = _sut.DoSomething("input");

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Method_ShouldTrimInput_WhenWhitespaceProvided()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var result = _sut.DoSomething("  input  ");

        Assert.Equal("result", result);
    }

    [Fact]
    public void Method_ShouldBeCaseInsensitive_WhenConfigured()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        var resultLower = _sut.DoSomething("input");
        var resultUpper = _sut.DoSomething("INPUT");

        Assert.Equal(resultLower, resultUpper);
    }

    [Fact]
    public void Method_ShouldSupportUnicodeCharacters()
    {
        _mockDependency.Setup(x => x.DoWork("café")).Returns("processed");

        var result = _sut.DoSomething("café");

        Assert.Equal("processed", result);
    }

    #endregion

    #region Dependency Behavior Verification

    [Fact]
    public void Method_ShouldCallDependencyOnce_WithExpectedInput()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("done");

        _sut.DoSomething("input");

        _mockDependency.Verify(x => x.DoWork("input"), Times.Once);
    }

    [Fact]
    public void Method_ShouldHandleUnexpectedDependencyException()
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Throws<InvalidOperationException>();

        var ex = Assert.Throws<ServiceOperationException>(() => _sut.DoSomething("input"));
        Assert.Contains("unexpected error", ex.Message.ToLower());
    }

    #endregion

    #region Logging

    [Fact]
    public void Method_ShouldLogInformation_WhenCalled()
    {
        _mockDependency.Setup(x => x.DoWork("input")).Returns("result");

        _sut.DoSomething("input");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Called DoSomething")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Theory Tests (InlineData & MemberData)

    /// <summary>
    /// Verifies that the service returns expected results for a variety of valid inputs using InlineData.
    /// </summary>
    [Theory]
    [InlineData("input", "expected")]
    [InlineData("  input  ", "expected")]
    [InlineData("INPUT", "expected")]
    [InlineData("café", "processed")]
    public void Method_ShouldReturnExpected_ForValidInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(It.IsAny<string>())).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that the service transforms multiple inputs into the expected results using MemberData.
    /// </summary>
    [Theory]
    [MemberData(nameof(ServiceTestData.ComplexInputCases), MemberType = typeof(ServiceTestData))]
    public void Method_ShouldReturnExpected_ForMemberDataInputs(string input, string expected)
    {
        _mockDependency.Setup(x => x.DoWork(input)).Returns(expected);

        var result = _sut.DoSomething(input);

        Assert.Equal(expected, result);
    }

    #endregion
}

/// <summary>
/// Provides reusable test data for [MemberData] scenarios.
/// </summary>
public static class ServiceTestData
{
    public static IEnumerable<object[]> ComplexInputCases =>
        new List<object[]>
        {
            new object[] { "a", "resultA" },
            new object[] { "b", "resultB" },
            new object[] { "C", "resultC" }
        };
}
