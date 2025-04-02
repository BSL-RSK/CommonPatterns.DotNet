# 🔢 EnumAssertionExtensions Examples

These examples demonstrate how to use custom enum assertion extensions to validate input values or enum safety in tests.

---

## ✅ ShouldBeDefinedEnum (string)

```csharp
public enum Status
{
    Draft,
    Published,
    Archived
}

[Fact]
public void Input_ShouldBeValid_EnumName()
{
    var input = "published";

    input.ShouldBeDefinedEnum<Status>();
}

[Fact]
public void Input_ShouldFail_WhenNotMatchingEnum()
{
    var input = "deleted";

    Action act = () => input.ShouldBeDefinedEnum<Status>();
    act.Should().Throw<FluentAssertions.Execution.AssertionFailedException>();
}
```

---

## ✅ ShouldBeValidEnumValue (enum value)

```csharp
[Fact]
public void Enum_ShouldBeRecognized_WhenDefined()
{
    var status = Status.Archived;

    status.ShouldBeValidEnumValue();
}

[Fact]
public void Enum_ShouldFail_WhenOutOfRange()
{
    var invalid = (Status)999;

    Action act = () => invalid.ShouldBeValidEnumValue();
    act.Should().Throw<FluentAssertions.Execution.AssertionFailedException>();
}
```

---

## ✅ Summary

Use these enum extensions when:

- Parsing strings into enums (e.g., user input, query parameters)
- Validating that values returned from external systems are defined in your code
- Guarding against accidental enum misuse or future expansion

➡️ They provide readable, assertive ways to handle enums safely in tests.
