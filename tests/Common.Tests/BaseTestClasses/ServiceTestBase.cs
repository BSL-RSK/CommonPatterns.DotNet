using Microsoft.Extensions.Logging;
using Moq;

namespace Common.Tests.BaseTestClasses;

/// <summary>
/// A reusable base class for unit testing services with common dependencies.
/// </summary>
/// <typeparam name="TService">The type of the service under test.</typeparam>
public abstract class ServiceTestBase<TService>
{
    /// <summary>
    /// Gets the mock logger for the service.
    /// </summary>
    protected Mock<ILogger<TService>> MockLogger { get; } = new();

    /// <summary>
    /// Gets the system under test (SUT).
    /// </summary>
    protected TService Sut { get; set; } = default!;

    /// <summary>
    /// Replaces the SUT with a new instance (if needed).
    /// </summary>
    /// <param name="instance">The new service instance.</param>
    protected void SetSut(TService instance)
    {
        Sut = instance;
    }
}