# 🧪 HttpContextHelper Usage Examples

This file shows how to use `HttpContextHelper` to inject a mocked `HttpContext` into controller or middleware tests.

---

## ✅ Creating a Mocked Context with User ID

```csharp
var context = HttpContextHelper.CreateMockContext("user-123");
```

This sets up:

- A `ClaimsPrincipal` with `ClaimTypes.NameIdentifier = "user-123"`
- A default name claim as "Test User"

---

## 🔐 Adding Roles to Simulate Authorization

```csharp
var context = HttpContextHelper.CreateMockContext("admin-user", new[] { "Admin", "Manager" });
```

Use this to test role-based logic like:

```csharp
User.IsInRole("Admin")
```

---

## 🧪 Controller Example

```csharp
[Fact]
public void Controller_ShouldAccessUserId_FromHttpContext()
{
    // Arrange
    var context = HttpContextHelper.CreateMockContext("user-42");
    var controller = new MyController();
    controller.ControllerContext = new ControllerContext
    {
        HttpContext = context
    };

    // Act
    var result = controller.GetUserId();

    // Assert
    Assert.Equal("user-42", result);
}
```

---

## 🧪 Middleware Example

```csharp
[Fact]
public async Task Middleware_ShouldLogUserRole()
{
    // Arrange
    var context = HttpContextHelper.CreateMockContext("test-id", new[] { "Developer" });
    var middleware = new RoleLoggingMiddleware(_ => Task.CompletedTask);

    // Act
    await middleware.Invoke(context);

    // Assert (verify log or captured role)
}
```

---

## ✅ Summary

Use `HttpContextHelper` to:

- 🔐 Simulate authenticated users
- 🔄 Inject roles and claims for security testing
- 🎯 Write isolated, expressive tests for user-aware logic

➡️ Especially useful in controller, middleware, and API authorization tests.
