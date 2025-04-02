# 🧪 MockLogger Usage Examples

This file shows how to use the `MockLogger` utility to simplify testing log output from services or controllers.

---

## ✅ Basic Logging Verification

```csharp
public class LoggingService
{
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    public void DoWork()
    {
        _logger.LogInformation("Work started.");
    }
}

[Fact]
public void DoWork_ShouldLogInformation()
{
    // Arrange
    var loggerMock = MockLogger.Create<LoggingService>();
    var service = new LoggingService(loggerMock.Object);

    // Act
    service.DoWork();

    // Assert
    loggerMock.VerifyLogContains("Work started.");
}
```

---

## 🔐 Verifying Log Level

```csharp
public class SecureService
{
    private readonly ILogger<SecureService> _logger;

    public SecureService(ILogger<SecureService> logger)
    {
        _logger = logger;
    }

    public void DenyAccess()
    {
        _logger.LogWarning("Access denied for user.");
    }
}

[Fact]
public void DenyAccess_ShouldLogWarning()
{
    var logger = MockLogger.Create<SecureService>();
    var service = new SecureService(logger.Object);

    service.DenyAccess();

    logger.VerifyLogContains("Access denied", LogLevel.Warning);
}
```

---

## ✅ Summary

Use `MockLogger` to:

- ✅ Quickly verify that log messages were emitted
- 🔍 Ensure logs contain the expected content
- 🎯 Assert the correct log level was used

➡️ Makes testing logging behavior simple, expressive, and centralized.
