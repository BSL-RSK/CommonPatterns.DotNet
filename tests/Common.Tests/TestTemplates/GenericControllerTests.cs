using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A reusable template for testing ASP.NET Core API controllers.
/// 
/// ✅ Includes:
/// - Successful action results
/// - BadRequest scenarios
/// - Null or invalid model inputs
/// - Dependency and logger mocking
/// 
/// 🔁 What to replace:
/// - Replace <c>SomeController</c>, <c>ISomeService</c>, and <c>DoWork</c> with your actual controller, service, and method names
/// </summary>
public class GenericControllerTests
{
    #region Fields & Setup

    private readonly Mock<ISomeService> _mockService;
    private readonly Mock<ILogger<SomeController>> _mockLogger;
    private readonly SomeController _sut;

    /// <summary>
    /// Initializes the controller with mocked dependencies.
    /// </summary>
    public GenericControllerTests()
    {
        _mockService = new Mock<ISomeService>();
        _mockLogger = new Mock<ILogger<SomeController>>();
        _sut = new SomeController(_mockService.Object, _mockLogger.Object);
    }

    #endregion

    #region Success Scenarios

    /// <summary>
    /// Verifies the controller returns 200 OK with expected result when service returns data.
    /// </summary>
    [Fact]
    public void DoWork_ShouldReturnOk_WhenSuccessful()
    {
        _mockService.Setup(x => x.DoWork()).Returns("success");

        var result = _sut.DoWork();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("success", okResult.Value);
    }

    #endregion

    #region Failure Scenarios

    /// <summary>
    /// Verifies the controller returns BadRequest when null input is passed.
    /// </summary>
    [Fact]
    public void DoWork_ShouldReturnBadRequest_WhenInputIsNull()
    {
        var result = _sut.DoWorkWithInput(null);

        Assert.IsType<BadRequestResult>(result);
    }

    /// <summary>
    /// Verifies the controller returns NotFound when service returns null.
    /// </summary>
    [Fact]
    public void DoWork_ShouldReturnNotFound_WhenNoDataReturned()
    {
        _mockService.Setup(x => x.DoWork()).Returns((string)null!);

        var result = _sut.DoWork();

        Assert.IsType<NotFoundResult>(result);
    }

    /// <summary>
    /// Verifies the controller logs error and returns 500 when exception occurs.
    /// </summary>
    [Fact]
    public void DoWork_ShouldReturnServerError_WhenExceptionThrown()
    {
        _mockService.Setup(x => x.DoWork()).Throws(new Exception("boom"));

        var result = _sut.DoWork();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("An error occurred.", objectResult.Value);
    }

    #endregion
}

/// <summary>
/// Example controller to match test template. Replace with your real controller.
/// </summary>
public class SomeController : ControllerBase
{
    private readonly ISomeService _service;
    private readonly ILogger<SomeController> _logger;

    public SomeController(ISomeService service, ILogger<SomeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult DoWork()
    {
        try
        {
            var result = _service.DoWork();
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to do work");
            return StatusCode(500, "An error occurred.");
        }
    }

    [HttpPost]
    public IActionResult DoWorkWithInput([FromBody] string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return BadRequest();

        var result = _service.DoWork();
        return Ok(result);
    }
}

/// <summary>
/// Example service interface to match test template.
/// </summary>
public interface ISomeService
{
    string DoWork();
}
