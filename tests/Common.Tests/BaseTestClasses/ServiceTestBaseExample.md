# 🧪 ServiceTestBase Example

This guide demonstrates how to use `ServiceTestBase<TService>` to simplify setup and enforce consistency across service unit tests.

---

## ✅ Why Use This?

This abstract base class:

- 📦 Provides a pre-configured `Mock<ILogger<T>>`
- 🧪 Gives you a consistent way to assign the system under test (`Sut`)
- 🧼 Reduces boilerplate in every service test file

Use this when your services depend on logging or are built using dependency injection.

---

## 💡 Example Scenario

You have a service like this:

```csharp
public class NotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public void Send(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.");

        _logger.LogInformation("Sending message: {Message}", message);
    }
}
```

---

## 🧪 Unit Test Using `ServiceTestBase<T>`

```csharp
public class NotificationServiceTests : ServiceTestBase<NotificationService>
{
    public NotificationServiceTests()
    {
        SetSut(new NotificationService(MockLogger.Object));
    }

    [Fact]
    public void Send_ShouldThrow_WhenMessageIsEmpty()
    {
        var act = () => Sut.Send("");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Send_ShouldLogMessage_WhenValid()
    {
        Sut.Send("Hello");

        MockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Sending message: Hello")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
    }
}
```

---

## ✅ Summary

Use `ServiceTestBase<T>` when:

- Your services use `ILogger<T>` or other shared dependencies
- You want clean, consistent test structure across your service tests
- You need a base test class for reuse across multiple test projects

➡️ Keeps your test setup lightweight and your intent clear.
