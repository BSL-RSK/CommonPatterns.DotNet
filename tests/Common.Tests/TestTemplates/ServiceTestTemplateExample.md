# 🧪 GenericServiceTests Example

This file provides a full working example inspired by `GenericServiceTests.cs` in the `TestTemplates` folder.

It demonstrates how to thoroughly test a service with:

- Dependency mocking
- Edge case handling
- Logging checks
- Inline and MemberData theory support

---

## ✅ Service and Interface Under Test

```csharp
public interface IUserProcessor
{
    string ProcessUsername(string? input);
}

public class UserProcessor : IUserProcessor
{
    private readonly INameFormatter _formatter;
    private readonly ILogger<UserProcessor> _logger;

    public UserProcessor(INameFormatter formatter, ILogger<UserProcessor> logger)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        _logger = logger;
    }

    public string ProcessUsername(string? input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Username cannot be empty.");

        var cleaned = input.Trim().ToLowerInvariant();

        var result = _formatter.Format(cleaned);

        if (result == null)
            return string.Empty;

        _logger.LogInformation("Processed username");
        return result;
    }
}

public interface INameFormatter
{
    string? Format(string name);
}
```

---

## ✅ Corresponding Tests

```csharp
public class UserProcessorTests
{
    private readonly Mock<INameFormatter> _mockFormatter;
    private readonly Mock<ILogger<UserProcessor>> _mockLogger;
    private readonly UserProcessor _sut;

    public UserProcessorTests()
    {
        _mockFormatter = new Mock<INameFormatter>();
        _mockLogger = new Mock<ILogger<UserProcessor>>();
        _sut = new UserProcessor(_mockFormatter.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenFormatterIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new UserProcessor(null!, _mockLogger.Object));
    }

    [Fact]
    public void ProcessUsername_ShouldThrow_WhenInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.ProcessUsername(null));
    }

    [Fact]
    public void ProcessUsername_ShouldThrow_WhenInputIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => _sut.ProcessUsername("   "));
    }

    [Fact]
    public void ProcessUsername_ShouldReturnFormattedValue_WhenValid()
    {
        _mockFormatter.Setup(f => f.Format("john")).Returns("Mr. John");

        var result = _sut.ProcessUsername(" John ");

        Assert.Equal("Mr. John", result);
    }

    [Fact]
    public void ProcessUsername_ShouldReturnEmpty_WhenFormatterReturnsNull()
    {
        _mockFormatter.Setup(f => f.Format(It.IsAny<string>())).Returns((string?)null);

        var result = _sut.ProcessUsername("anything");

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ProcessUsername_ShouldLog_WhenCalled()
    {
        _mockFormatter.Setup(f => f.Format(It.IsAny<string>())).Returns("formatted");

        _sut.ProcessUsername("user");

        _mockLogger.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Processed username")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    public static IEnumerable<object[]> ValidUsernames => new List<object[]>
    {
        new object[] { "JOHN", "Mr. John" },
        new object[] { " jane  ", "Ms. Jane" },
        new object[] { "max", "Dr. Max" }
    };

    [Theory]
    [MemberData(nameof(ValidUsernames))]
    public void ProcessUsername_ShouldReturnExpectedResult_WhenUsingMemberData(string input, string expected)
    {
        _mockFormatter.Setup(f => f.Format(It.IsAny<string>())).Returns(expected);

        var result = _sut.ProcessUsername(input);

        Assert.Equal(expected, result);
    }
}
```
