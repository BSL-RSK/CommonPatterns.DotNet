# 🧪 MockUserContext Usage Examples

This file demonstrates how to use `MockUserContext` to simulate an authenticated user via `IHttpContextAccessor`.

Useful when testing services or components that extract user information from the current HTTP context.

---

## ✅ Basic User ID Access

```csharp
public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor accessor)
    {
        _httpContextAccessor = accessor;
    }

    public string? GetUserId() =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}

[Fact]
public void GetUserId_ShouldReturnExpectedUserId()
{
    var userContext = MockUserContext.Create("abc-123");
    var service = new UserService(userContext.Object);

    var result = service.GetUserId();

    Assert.Equal("abc-123", result);
}
```

---

## 🔐 Simulate Roles for Authorization

```csharp
public class AuthorizationService
{
    private readonly IHttpContextAccessor _accessor;

    public AuthorizationService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public bool IsInRole(string role)
    {
        return _accessor.HttpContext?.User.IsInRole(role) ?? false;
    }
}

[Fact]
public void IsInRole_ShouldReturnTrue_WhenUserHasRole()
{
    var userContext = MockUserContext.Create("user1", new[] { "Admin", "Manager" });
    var service = new AuthorizationService(userContext.Object);

    var isAdmin = service.IsInRole("Admin");

    Assert.True(isAdmin);
}
```

---

## 📋 Use Cases

Use `MockUserContext` when:

- Your service or controller logic relies on `IHttpContextAccessor`
- You want to simulate a specific authenticated user
- You need to test role-based conditions (e.g., `IsInRole("Admin")`)

It's ideal for:

- ✅ User identity and auditing
- ✅ Claims extraction
- ✅ Role-based access testing

---

## ✅ Summary

Use `MockUserContext` to easily inject authenticated user state into any test involving:

- Controllers
- Services
- Middleware or filters

➡️ Great for eliminating boilerplate when testing identity-sensitive logic.
