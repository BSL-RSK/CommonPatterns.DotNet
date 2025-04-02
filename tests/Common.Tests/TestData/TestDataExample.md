# 🧪 TestData Usage Examples

Real-world usage scenarios for leveraging the contents of the `TestData` folder in parameterized tests and mock validation.

---

## ✅ Using `MemberDataProviders.BasicValidationCases`

```csharp
public class StringValidationTests
{
    private readonly SomeValidator _validator = new SomeValidator();

    [Theory]
    [MemberData(nameof(MemberDataProviders.BasicValidationCases), MemberType = typeof(MemberDataProviders))]
    public void IsValidString_ShouldReturnExpected(string input, bool expected)
    {
        var result = _validator.IsValidString(input);

        Assert.Equal(expected, result);
    }
}
```

---

## 🧍 Using `MemberDataProviders.UserRecords`

```csharp
public class UserAccessServiceTests
{
    [Theory]
    [MemberData(nameof(MemberDataProviders.UserRecords), MemberType = typeof(MemberDataProviders))]
    public void IsActiveUser_ShouldReflectCorrectStatus(TestUser user, bool expected)
    {
        var service = new UserAccessService();

        var result = service.IsActiveUser(user);

        Assert.Equal(expected, result);
    }
}
```

---

## 📦 Using `example.json` as a deserialization input

```csharp
public class JsonModelBindingTests
{
    [Fact]
    public void ShouldDeserializeExampleUserJson_Correctly()
    {
        var json = File.ReadAllText("TestData/example.json");
        var user = JsonSerializer.Deserialize<TestUser>(json);

        Assert.Equal("test-user-123", user!.Id);
        Assert.Contains("Admin", user.Roles);
        Assert.True(user.IsActive);
    }
}
```

---

## ✅ Summary

- Use `MemberDataProviders` for clean, reusable `[Theory]` inputs.
- Keep structured example files like `example.json` for integration-style tests.
- Centralize shared test inputs to reduce duplication and boost test readability.
