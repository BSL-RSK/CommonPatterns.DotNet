# 🌐 HttpResponseAssertionExtensions Examples

These examples demonstrate how to use `HttpResponseAssertionExtensions` to validate `HttpResponseMessage` results in API and integration tests.

---

## ✅ Assert Status Code

```csharp
[Fact]
public void Response_ShouldHaveExpectedStatusCode()
{
    var response = new HttpResponseMessage(HttpStatusCode.OK);

    response.ShouldHaveStatusCode(HttpStatusCode.OK);
}
```

---

## ✅ Assert Header Presence

```csharp
[Fact]
public void Response_ShouldContainHeader()
{
    var response = new HttpResponseMessage();
    response.Headers.Add("X-Correlation-ID", "abc123");

    response.ShouldContainHeader("X-Correlation-ID");
}
```

---

## ✅ Assert Body Content

```csharp
[Fact]
public async Task Response_ShouldContainExpectedText()
{
    var content = new StringContent("{"message": "All good"}");
    var response = new HttpResponseMessage
    {
        Content = content
    };

    await response.ShouldContainInBodyAsync("All good");
}
```

---

## ✅ Assert Empty Body

```csharp
[Fact]
public async Task Response_ShouldHaveEmptyBody()
{
    var response = new HttpResponseMessage
    {
        Content = new StringContent("")
    };

    await response.ShouldHaveEmptyBodyAsync();
}
```

---

## ✅ Summary

Use these helpers when testing:

- API endpoints
- Controller responses
- Typed HTTP client wrappers

➡️ Makes your assertions more expressive and focused on behavior.
