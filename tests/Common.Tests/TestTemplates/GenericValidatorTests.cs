using Xunit;
using FluentValidation;
using FluentValidation.Results;

namespace Common.Tests.TestTemplates;

/// <summary>
/// A reusable unit test template for validating input models using FluentValidation or similar frameworks.
/// 
/// ✅ Includes:
/// - Valid and invalid input cases
/// - Null and empty checks
/// - Boundary value assertions
/// - MemberData support for multiple scenarios
/// 
/// 🔁 What to replace:
/// - Replace <c>SomeModel</c>, <c>SomeModelValidator</c> with your actual model and validator types
/// </summary>
public class GenericValidatorTests
{
    #region Fields & Setup

    private readonly SomeModelValidator _validator;

    /// <summary>
    /// Initializes a new instance of the validator to test.
    /// </summary>
    public GenericValidatorTests()
    {
        _validator = new SomeModelValidator();
    }

    #endregion

    #region Valid Input Tests

    /// <summary>
    /// Verifies that a fully valid model passes validation.
    /// </summary>
    [Fact]
    public void Validate_ShouldPass_WhenModelIsValid()
    {
        var model = new SomeModel { Name = "Valid Name", Age = 25 };

        var result = _validator.Validate(model);

        Assert.True(result.IsValid);
    }

    #endregion

    #region Invalid Input Tests

    /// <summary>
    /// Verifies that null model input returns a validation failure.
    /// </summary>
    [Fact]
    public void Validate_ShouldFail_WhenModelIsNull()
    {
        var result = _validator.Validate((SomeModel)null!);

        Assert.False(result.IsValid);
    }

    /// <summary>
    /// Verifies that missing required properties result in validation failures.
    /// </summary>
    [Fact]
    public void Validate_ShouldFail_WhenRequiredFieldsAreMissing()
    {
        var model = new SomeModel();

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Name");
    }

    /// <summary>
    /// Verifies that boundary values are enforced properly.
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(151)]
    public void Validate_ShouldFail_WhenAgeOutOfRange(int age)
    {
        var model = new SomeModel { Name = "Valid", Age = age };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Age");
    }

    #endregion

    #region MemberData Tests

    /// <summary>
    /// Verifies multiple invalid name inputs using shared member data.
    /// 🔗 Uses <see cref="ValidatorTestData.InvalidNames"/>
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidatorTestData.InvalidNames), MemberType = typeof(ValidatorTestData))]
    public void Validate_ShouldFail_WhenNameIsInvalid(string name)
    {
        var model = new SomeModel { Name = name, Age = 30 };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Name");
    }

    #endregion
}

/// <summary>
/// Provides member data for validator edge case tests.
/// </summary>
public static class ValidatorTestData
{
    /// <summary>
    /// Invalid name scenarios (null, empty, whitespace).
    /// </summary>
    public static IEnumerable<object[]> InvalidNames =>
        new List<object[]>
        {
            new object[] { null! },
            new object[] { "" },
            new object[] { "     " }
        };
}

/// <summary>
/// Example model to validate. Replace with your own domain model.
/// </summary>
public class SomeModel
{
    public string? Name { get; set; }
    public int Age { get; set; }
}

/// <summary>
/// Example FluentValidator for SomeModel. Replace with your own rules.
/// </summary>
public class SomeModelValidator : AbstractValidator<SomeModel>
{
    public SomeModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Age).InclusiveBetween(1, 150);
    }
}
