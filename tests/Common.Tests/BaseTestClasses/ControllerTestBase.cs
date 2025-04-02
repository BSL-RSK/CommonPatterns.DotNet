using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Common.Tests.BaseTestClasses;

/// <summary>
/// A reusable base class for testing ASP.NET Core controllers.
/// Provides mocked HttpContext and user identity setup.
/// </summary>
public abstract class ControllerTestBase
{
    /// <summary>
    /// Mocks the controller context with a user containing the given claims.
    /// </summary>
    /// <param name="claims">A list of claims to assign to the mock user.</param>
    /// <returns>A ControllerContext with mocked HttpContext and user.</returns>
    protected static ControllerContext CreateControllerContextWithClaims(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = principal
        };

        return new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    /// <summary>
    /// Returns a default ControllerContext with a user containing a single ID claim.
    /// </summary>
    /// <param name="userId">The user ID to assign to the mock user.</param>
    protected static ControllerContext CreateControllerContextWithUserId(string userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

        return CreateControllerContextWithClaims(claims);
    }
}