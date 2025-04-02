# 🧪 GenericControllerTests Example

This file provides a full example matching the structure of `GenericControllerTests.cs` in the `TestTemplates` folder.

It demonstrates how to test a controller with:
- Dependency injection
- Mocked service responses
- HTTP response assertions
- Logger verification

---

## ✅ Controller and Service Under Test

```csharp
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _service;
    private readonly ILogger<UserProfileController> _logger;

    public UserProfileController(IUserProfileService service, ILogger<UserProfileController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("/api/profile")]
    public IActionResult GetProfile()
    {
        try
        {
            var result = _service.GetProfile();
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve user profile");
            return StatusCode(500, "An error occurred while retrieving the profile.");
        }
    }

    [HttpPost("/api/profile/update")]
    public IActionResult UpdateProfile([FromBody] string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return BadRequest();

        var result = _service.UpdateUsername(username);
        return Ok(result);
    }
}

public interface IUserProfileService
{
    string GetProfile();
    string UpdateUsername(string username);
}
```

---

## 🧪 Breakdown of Each Test

### ✅ GetProfile_ShouldReturnOk_WhenProfileExists

```csharp
[Fact]
public void GetProfile_ShouldReturnOk_WhenProfileExists()
{
    _mockService.Setup(x => x.GetProfile()).Returns("John Doe");

    var result = _sut.GetProfile();

    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal("John Doe", okResult.Value);
}
```
✅ Verifies that a valid service result returns a `200 OK` with the profile.

---

### ❌ UpdateProfile_ShouldReturnBadRequest_WhenInputIsNull

```csharp
[Fact]
public void UpdateProfile_ShouldReturnBadRequest_WhenInputIsNull()
{
    var result = _sut.UpdateProfile(null);

    Assert.IsType<BadRequestResult>(result);
}
```
✅ Simulates a null username input and expects a `400 Bad Request`.

---

### ❌ GetProfile_ShouldReturnNotFound_WhenNoDataReturned

```csharp
[Fact]
public void GetProfile_ShouldReturnNotFound_WhenNoDataReturned()
{
    _mockService.Setup(x => x.GetProfile()).Returns((string)null!);

    var result = _sut.GetProfile();

    Assert.IsType<NotFoundResult>(result);
}
```
✅ Simulates a null return from the service and expects a `404 Not Found`.

---

### 💥 GetProfile_ShouldReturnServerError_WhenExceptionThrown

```csharp
[Fact]
public void GetProfile_ShouldReturnServerError_WhenExceptionThrown()
{
    _mockService.Setup(x => x.GetProfile()).Throws(new Exception("DB connection failed"));

    var result = _sut.GetProfile();

    var objectResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(500, objectResult.StatusCode);
    Assert.Equal("An error occurred while retrieving the profile.", objectResult.Value);
}
```
✅ Simulates an exception and checks for a `500 Internal Server Error` response.

> You can also verify that `_logger.LogError(...)` was called using Moq.

---

## ✅ Summary

This test template is ideal for controller actions that:

- Return different HTTP status codes based on business logic or input
- Call services with mocked logic
- Log exceptions or other messages

Use it as a foundation for any controller with:
- Business logic delegation
- Error and null checking
- Input validation