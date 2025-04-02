# 🧪 ExceptionAssertionExtensions Examples

These examples demonstrate how to use the custom exception assertion helpers defined in `ExceptionAssertionExtensions.cs`.

They simplify testing for specific exception types, messages, and nested errors.

---

## ✅ Exact Exception and Message Match

```csharp
[Fact]
public void ShouldThrowWithMessage_ShouldMatchExactMessage()
{
    void Act() => throw new InvalidOperationException("Something went wrong");

    Act.ShouldThrowWithMessage<InvalidOperationException>("Something went wrong");
}
```

---

## 🧩 Partial Match on Exception Message

```csharp
[Fact]
public void ShouldThrowContaining_ShouldContainKeyword()
{
    void Act() => throw new ArgumentException("Parameter 'name' is required.");

    Act.ShouldThrowContaining<ArgumentException>("'name'");
}
```

---

## 🔗 Asserting Inner Exception Type

```csharp
[Fact]
public void ShouldHaveInnerException_ShouldMatchInnerType()
{
    void Act() => throw new InvalidOperationException("Failed", new FormatException("Bad format"));

    Act.ShouldHaveInnerException<InvalidOperationException, FormatException>();
}
```

---

## 🧠 Assert Inner Exception Message

```csharp
[Fact]
public void ShouldHaveInnerExceptionWithMessage_ShouldCheckInnerMessage()
{
    void Act() => throw new ApplicationException("Top-level", new NullReferenceException("Something was null"));

    Act.ShouldHaveInnerExceptionWithMessage<ApplicationException, NullReferenceException>("null");
}
```

---

## 🧵 Aggregate Exception with Inner Match

```csharp
[Fact]
public void ShouldThrowAggregateWithInner_ShouldContainExpectedType()
{
    void Act() => throw new AggregateException(
        new InvalidOperationException(),
        new ArgumentNullException()
    );

    Act.ShouldThrowAggregateWithInner<ArgumentNullException>();
}
```

---

## 🎯 Exact Exception Type (No Inheritance Match)

```csharp
[Fact]
public void ShouldThrowExactly_ShouldMatchTypeExactly()
{
    void Act() => throw new FormatException("Specific format error");

    Act.ShouldThrowExactly<FormatException>();
}
```

---

## ✅ Summary

Use these extensions to:

- Assert that specific exceptions are thrown
- Check messages for clarity or localization
- Ensure nested exceptions and aggregate types are handled as expected

➡️ These methods make exception validation expressive and robust in any test suite.
