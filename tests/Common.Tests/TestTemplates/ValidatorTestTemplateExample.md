# 🧪 GenericValidatorTests Example

This example illustrates how to unit test a model validator using FluentValidation and Xunit. It matches the structure of `GenericValidatorTests.cs` in the `TestTemplates` folder.

The validator checks a `UserInputModel` with two rules:

- `Username` must not be null or empty
- `Age` must be between 1 and 120

---

## ✅ Model & Validator

```csharp
public class UserInputModel
{
    public string? Username { get; set; }
    public int Age { get; set; }
}

public class UserInputModelValidator : AbstractValidator<UserInputModel>
{
    public UserInputModelValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Age).InclusiveBetween(1, 120);
    }
}
```

---

## 🧪 Breakdown of Tests

### ✅ Validate_ShouldPass_WhenModelIsValid

```csharp
[Fact]
public void Validate_ShouldPass_WhenModelIsValid()
{
    var model = new UserInputModel { Username = "johndoe", Age = 30 };
    var validator = new UserInputModelValidator();

    var result = validator.Validate(model);

    Assert.True(result.IsValid);
}
```

✅ Asserts that valid inputs pass without errors.

---

### ❌ Validate_ShouldFail_WhenModelIsNull

```csharp
[Fact]
public void Validate_ShouldFail_WhenModelIsNull()
{
    var validator = new UserInputModelValidator();
    var result = validator.Validate((UserInputModel)null!);

    Assert.False(result.IsValid);
}
```

✅ Validates that passing `null` returns an invalid result.

---

### ❌ Validate_ShouldFail_WhenRequiredFieldsAreMissing

```csharp
[Fact]
public void Validate_ShouldFail_WhenRequiredFieldsAreMissing()
{
    var model = new UserInputModel();
    var validator = new UserInputModelValidator();

    var result = validator.Validate(model);

    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.PropertyName == "Username");
}
```

✅ Checks that missing properties trigger validation errors.

---

### 📏 Validate_ShouldFail_WhenAgeOutOfRange (Theory)

```csharp
[Theory]
[InlineData(-5)]
[InlineData(0)]
[InlineData(150)]
public void Validate_ShouldFail_WhenAgeOutOfRange(int age)
{
    var model = new UserInputModel { Username = "valid", Age = age };
    var validator = new UserInputModelValidator();

    var result = validator.Validate(model);

    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.PropertyName == "Age");
}
```

✅ Uses [Theory] to verify various out-of-range age values.

---

### 🔁 Validate_ShouldFail_WhenUsernameIsInvalid (MemberData)

```csharp
public static IEnumerable<object[]> InvalidUsernames =>
    new List<object[]>
    {
        new object[] { null! },
        new object[] { "" },
        new object[] { "    " }
    };

[Theory]
[MemberData(nameof(InvalidUsernames))]
public void Validate_ShouldFail_WhenUsernameIsInvalid(string username)
{
    var model = new UserInputModel { Username = username, Age = 35 };
    var validator = new UserInputModelValidator();

    var result = validator.Validate(model);

    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.PropertyName == "Username");
}
```

✅ Reuses common edge cases for invalid username values.

---

## ✅ Summary

Use this structure to test validators that:

- Validate both required and optional fields
- Rely on length, range, or custom logic
- Need coverage of boundary conditions
- Are implemented using FluentValidation or similar frameworks
