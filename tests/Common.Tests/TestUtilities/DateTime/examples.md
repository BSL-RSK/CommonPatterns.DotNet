# 🧪 DateTimeStub Test Examples

This file contains usage examples for `DateTimeStub`, demonstrating how to test **real-world services** that depend on time.

Each example shows a scenario where time is involved (e.g. token generation, expiry checks, retry windows), and how `DateTimeStub` makes these tests deterministic.

---

## ✅ Freeze time for predictable token generation

Use `DateTimeStub` to fix the system clock so token values are always the same during testing.

```csharp
public class TokenService
{
    private readonly IDateTimeProvider _clock;
    public TokenService(IDateTimeProvider clock) => _clock = clock;

    public string GenerateToken() => $"TOKEN-{_clock.UtcNow:yyyyMMddHHmmss}";
}

[Fact]
public void GenerateToken_ShouldIncludeFixedTimestamp()
{
    var clock = new DateTimeStub();
    clock.Freeze(new DateTime(2025, 04, 01, 12, 0, 0));

    var service = new TokenService(clock);
    var token = service.GenerateToken();

    Assert.Equal("TOKEN-20250401120000", token);
}
```

---

## ⏳ Test token expiration logic

Simulate tokens that expire 30 minutes after issuance.

```csharp
public class AuthService
{
    private readonly IDateTimeProvider _clock;
    public AuthService(IDateTimeProvider clock) => _clock = clock;

    public bool IsTokenExpired(DateTime issuedAt)
    {
        return _clock.UtcNow > issuedAt.AddMinutes(30);
    }
}

[Fact]
public void Token_ShouldBeExpired_After30Minutes()
{
    var clock = new DateTimeStub();
    var issuedAt = new DateTime(2024, 01, 01, 9, 0, 0);

    clock.Freeze(issuedAt);
    var service = new AuthService(clock);
    Assert.False(service.IsTokenExpired(issuedAt));

    clock.Advance(TimeSpan.FromMinutes(31));
    Assert.True(service.IsTokenExpired(issuedAt));
}
```

---

## ⏱ Test retry backoff based on clock

Simulate advancing time to retry a failed operation.

```csharp
public class RetryPolicy
{
    private readonly IDateTimeProvider _clock;
    private DateTime? _lastAttempt;

    public RetryPolicy(IDateTimeProvider clock) => _clock = clock;

    public bool CanRetry()
    {
        if (_lastAttempt == null)
        {
            _lastAttempt = _clock.UtcNow;
            return true;
        }
        return _clock.UtcNow > _lastAttempt.Value.AddSeconds(10);
    }
}

[Fact]
public void CanRetry_ShouldRespectCooldownPeriod()
{
    var clock = new DateTimeStub();
    clock.Freeze(new DateTime(2024, 01, 01, 10, 0, 0));
    var retry = new RetryPolicy(clock);

    Assert.True(retry.CanRetry());   // first attempt
    Assert.False(retry.CanRetry());  // too soon

    clock.Advance(TimeSpan.FromSeconds(11));
    Assert.True(retry.CanRetry());   // enough time passed
}
```

---

## 🕐 Handle local time logic safely

Sometimes your app needs local time (e.g. for logging or display).

```csharp
public class Logger
{
    private readonly IDateTimeProvider _clock;
    public Logger(IDateTimeProvider clock) => _clock = clock;

    public string GetLogHeader() => $"[{_clock.Now:HH:mm}]";
}

[Fact]
public void GetLogHeader_ShouldUseLocalTime()
{
    var utc = new DateTime(2024, 01, 01, 12, 0, 0, DateTimeKind.Utc);
    var clock = new DateTimeStub();
    clock.Freeze(utc);

    var logger = new Logger(clock);
    var header = logger.GetLogHeader();

    Assert.Contains(DateTimeStub.UtcNow.ToLocalTime().ToString("HH:mm"), header);
}
```
