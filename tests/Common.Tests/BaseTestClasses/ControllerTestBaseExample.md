# 🧪 ControllerTestBase Example

This example demonstrates how to use `ControllerTestBase` to simplify the setup of authenticated controller tests with `HttpContext` and user claims.

---

## ✅ Why This Exists

While `TestTemplates` provide **copyable full test classes** for services/controllers/etc., this file exists to:

- 💡 Provide **shared test logic** (not templates)
- 🧼 Eliminate duplicate setup code for controller `HttpContext`
- 🧪 Inject custom claims into controller tests without rewriting identity setup

Ideal for projects that:

- Have many controller tests
- Require authenticated `HttpContext.User`
- Check claims-based authorization or identity-related behavior

---

## 🌍 Example Scenario

You have a controller like this:

```csharp
public class ProfileController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetMyId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(userId);
    }
}
```

You want to test that the controller reads the correct user ID.

---

## 🧪 Unit Test Using `ControllerTestBase`

```csharp
public class ProfileControllerTests : ControllerTestBase
{
    [Fact]
    public void GetMyId_ShouldReturnCurrentUserId()
    {
        // Arrange
        var controller = new ProfileController();
        controller.ControllerContext = CreateControllerContextWithUserId("user-123");

        // Act
        var result = controller.GetMyId();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().Be("user-123");
    }
}
```

---

## ✅ Summary

Use `ControllerTestBase` when you need to:

- Set up `ControllerContext` with mock `HttpContext`
- Simulate authenticated users with custom claims
- Keep your controller tests clean and DRY

➡️ Perfect for controller tests that depend on identity, headers, or request context.
