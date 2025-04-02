# 🧪 FluentAssertionExtensions Examples

Examples of using the custom extension methods provided in `FluentAssertionExtensions.cs`.
These help simplify expressive assertions for common types and structures.

---

## ✅ Asserting All Substrings in a String

```csharp
[Fact]
public void Message_ShouldContainAllKeywords()
{
    var message = "Welcome to the platform. Login successful. Access granted.";

    message.ShouldContainAll("Welcome", "Login", "Access");
}
```

---

## ✅ Validating Collections

```csharp
[Fact]
public void List_ShouldNotBeNullOrEmpty()
{
    var values = new[] { "A", "B" };

    values.ShouldNotBeNullOrEmpty();
}

[Fact]
public void List_ShouldContainAny_MatchingValue()
{
    var roles = new[] { "Admin", "User" };

    roles.ShouldContainAny("User", "Guest");
}

[Fact]
public void List_ShouldNotContainAny_DisallowedValues()
{
    var status = new[] { "Active", "Pending" };

    status.ShouldNotContainAny("Deleted", "Banned");
}
```

---

## ✅ Type and Casting Assertions

```csharp
[Fact]
public void Result_ShouldBeOfType_AndCast()
{
    object result = "a string";

    var casted = result.ShouldBeOfType<string>();
    casted.Should().StartWith("a");
}
```

---

## ✅ Collection Equality Without Order

```csharp
[Fact]
public void Values_ShouldBeEquivalentIgnoringOrder()
{
    var actual = new[] { "B", "A", "C" };
    var expected = new[] { "C", "B", "A" };

    actual.ShouldBeEquivalentToIgnoringOrder(expected);
}
```

---

## ✅ Single Item Collection Assertion

```csharp
[Fact]
public void ShouldHaveSingle_ShouldReturnTheOnlyElement()
{
    var list = new[] { "only" };

    var item = list.ShouldHaveSingle();
    item.Should().Be("only");
}
```

---

## ✅ Summary

These extensions:

- Keep assertions expressive and reusable
- Reduce repetition and improve test readability
- Fit naturally into FluentAssertions style

➡️ Use them freely to simplify and strengthen your test suite.
