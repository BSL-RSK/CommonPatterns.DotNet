using Microsoft.Extensions.Logging;
using Moq;

namespace Common.Tests.SharedMocks;

/// <summary>
/// Provides a reusable mock logger for capturing and verifying logs during tests.
/// </summary>
public static class MockLogger
{
    /// <summary>
    /// Creates a mock <see cref="ILogger{T}"/> for verifying log calls.
    /// </summary>
    /// <typeparam name="T">The type the logger is for.</typeparam>
    /// <returns>A configured <see cref="Mock{ILogger{T}}"/> instance.</returns>
    public static Mock<ILogger<T>> Create<T>()
    {
        return new Mock<ILogger<T>>();
    }

    /// <summary>
    /// Verifies that a log message containing a specific string was written.
    /// </summary>
    /// <typeparam name="T">The type the logger is for.</typeparam>
    /// <param name="mockLogger">The mock logger instance.</param>
    /// <param name="contains">Text expected to be present in the log message.</param>
    /// <param name="level">The expected <see cref="LogLevel"/>. Defaults to Information.</param>
    public static void VerifyLogContains<T>(this Mock<ILogger<T>> mockLogger, string contains, LogLevel level = LogLevel.Information)
    {
        mockLogger.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(contains)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }
}