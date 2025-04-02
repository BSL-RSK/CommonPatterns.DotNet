# ⏰ DateTime Utilities

This folder contains everything needed to replace direct calls to `DateTime.UtcNow` and `DateTime.Now` with a clean, testable abstraction.

## ✅ Why use this?

Direct use of `DateTime.UtcNow` in your application code:

- ❌ Makes unit testing difficult (you can't control the current time)
- ❌ Introduces flakiness in time-sensitive logic
- ❌ Hinders deterministic test results

### ✅ Solution: Use `IDateTimeProvider`

Instead of calling system time directly, inject an interface:

```csharp
public class MyService
{
    private readonly IDateTimeProvider _clock;

    public MyService(IDateTimeProvider clock)
    {
        _clock = clock;
    }

    public DateTime GetTimestamp() => _clock.UtcNow;
}
```

---

## 🧱 Components

### [`IDateTimeProvider.cs`](./IDateTimeProvider.cs)

Defines the contract:

```csharp
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateTime Now { get; }
}
```

---

### [`SystemDateTimeProvider.cs`](./SystemDateTimeProvider.cs)

Production implementation using the real system clock:

```csharp
public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
}
```

---

### [`DateTimeStub.cs`](./DateTimeStub.cs)

Test-only implementation that allows you to:

- ❄️ Freeze time
- ⏩ Advance time manually

```csharp
var clock = new DateTimeStub();
clock.Freeze(new DateTime(2024, 01, 01));

Assert.Equal("2024-01-01", clock.UtcNow.ToString("yyyy-MM-dd"));
```

Also implements `IDateTimeProvider` for seamless substitution.

---

## 🧪 Example Test Usage

See: [`examples.md`](./examples.md) for complete real-world test scenarios.

Covers:

- 🔐 Token generation with timestamps
- ⏳ Expiration logic with advancing time
- ⏱ Retry cooldown windows
- 🕐 Logging with local time formatting

---

## ✅ Summary

Always depend on `IDateTimeProvider` in application logic, and inject either:

| Context    | Use                      |
| ---------- | ------------------------ |
| Production | `SystemDateTimeProvider` |
| Unit Tests | `DateTimeStub`           |

Use this pattern to make time behavior predictable, testable, and future-proof.
