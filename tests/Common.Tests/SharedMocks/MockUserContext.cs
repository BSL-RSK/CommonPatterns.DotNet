using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Common.Tests.SharedMocks;

/// <summary>
/// Provides a reusable mock for <see cref="IHttpContextAccessor"/> with a customizable authenticated user.
/// </summary>
public static class MockUserContext
{
    /// <summary>
    /// Creates a mock <see cref="IHttpContextAccessor"/> that simulates an authenticated user.
    /// </summary>
    /// <param name="userId">The user ID (NameIdentifier claim).</param>
    /// <param name="roles">Optional roles to assign to the user.</param>
    /// <returns>A configured mock of <see cref="IHttpContextAccessor"/>.</returns>
    public static Mock<IHttpContextAccessor> Create(string userId = "test-user", string[]? roles = null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, "Test User")
        };

        if (roles != null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var identity = new ClaimsIdentity(claims, "mock");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = principal
        };

        var mockAccessor = new Mock<IHttpContextAccessor>();
        mockAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        return mockAccessor;
    }
}