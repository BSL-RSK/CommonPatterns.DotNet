using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Common.Tests.TestUtilities;

/// <summary>
/// Utility class for creating mocked <see cref="HttpContext"/> instances in tests.
/// </summary>
public static class HttpContextHelper
{
    /// <summary>
    /// Creates a mock <see cref="HttpContext"/> with an authenticated user and optional claims.
    /// </summary>
    /// <param name="userId">User ID to embed in the claim.</param>
    /// <param name="roles">Roles to assign to the mocked user.</param>
    /// <returns>A mocked <see cref="HttpContext"/> object.</returns>
    public static HttpContext CreateMockContext(string userId = "test-user", string[]? roles = null)
    {
        var identity = new ClaimsIdentity("TestAuthType");
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
        identity.AddClaim(new Claim(ClaimTypes.Name, "Test User"));

        if (roles != null)
        {
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }

        var claimsPrincipal = new ClaimsPrincipal(identity);

        var mockContext = new Mock<HttpContext>();
        mockContext.Setup(c => c.User).Returns(claimsPrincipal);

        return mockContext.Object;
    }
}
