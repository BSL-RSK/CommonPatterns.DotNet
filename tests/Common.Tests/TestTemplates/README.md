# 🔧 Test Templates

### 📂 Contents

Jump to the relevant test template:

- [`GenericServiceTests.cs`](./GenericServiceTests.cs) — service logic and edge cases ([example](./ServiceTestTemplateExample.md))
- [`GenericRepositoryTests.cs`](./GenericRepositoryTests.cs) — data access and filtering ([example](./RepositoryTestTemplateExample.md))
- [`GenericValidatorTests.cs`](./GenericValidatorTests.cs) — FluentValidation and input rules ([example](./ValidatorTestTemplateExample.md))
- [`GenericControllerTests.cs`](./GenericControllerTests.cs) — ASP.NET Core controller responses ([example](./ControllerTestTemplateExample.md))

A set of reusable test case templates for common edge cases:

- Null input handling
- Dependency injection validation
- Happy path tests
- Mocking patterns with Moq
- Data-driven testing with [Theory]

Use these as starting points when testing new services, repositories, or validators.

---

## 📦 Dependencies

Make sure the following NuGet packages are installed in your test project:

```bash
dotnet add package xunit
dotnet add package Moq
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.runner.visualstudio
```

---

## 🧪 Test Authoring Guide

When writing unit tests, aim to cover both expected ("happy path") and edge-case behaviors.

### ✅ What to Test

- Correct return values for valid input
- Exceptions for invalid/null input
- Calls to dependencies (using mocks)
- Logging behavior (e.g., ILogger logs expected messages)
- State changes (if applicable)
- Input formatting and sanitization
- Unicode and localization handling

### 🔍 Assertion Examples

- Assert.Equal(expected, actual) – exact match
- Assert.NotNull(obj) – object was created or returned
- Assert.Throws<ExceptionType>(...) – expected failure
- mock.Verify(...) – ensure a dependency method was called
- Assert.Contains("text", result) – match substring in output

### 🧠 Tips

- Use expressive test method names: Method_ShouldDo_ExpectedBehavior
- Keep each test focused on a single responsibility
- Favor [Fact] for single-case tests, [Theory] for data-driven tests
- Avoid testing internal implementation — test behavior and outcomes

---

### 🎯 [Fact] vs [Theory]

Use `[Fact]` when the test has no input parameters — it's a single scenario.  
Use `[Theory]` when you want to test multiple inputs using the same logic.  
Use `[MemberData]` with `[Theory]` when you need to supply complex or reusable data from a property or method.

#### ✅ [Fact] Example

```csharp
[Fact]
public void Method_ShouldReturnTrue_WhenConditionIsMet()
{
    var service = new SomeService();
    var result = service.CheckCondition();

    Assert.True(result);
}
```

#### ✅ [Theory] Example

```csharp
[Theory]
[InlineData("valid", true)]
[InlineData("invalid", false)]
public void Method_ShouldValidateInput_Correctly(string input, bool expected)
{
    var service = new SomeService();
    var result = service.Validate(input);

    Assert.Equal(expected, result);
}
```

✅ [MemberData] Example

```csharp
public static IEnumerable<object[]> InputData =>
    new List<object[]>
    {
        new object[] { "value1", "expected1" },
        new object[] { "value2", "expected2" }
    };

[Theory]
[MemberData(nameof(InputData))]
public void Method_ShouldReturnExpected_ForMultipleInputs(string input, string expected)
{
    var service = new SomeService();
    var result = service.Transform(input);

    Assert.Equal(expected, result);
}
```
