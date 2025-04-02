# 🕒 DateTimeAssertionExtensions Examples

These examples demonstrate how to use the custom `DateTimeAssertionExtensions` to improve readability and precision when testing time-sensitive logic.

---

## ✅ ShouldBeCloseTo

```csharp
[Fact]
public void Timestamp_ShouldBeCloseTo_ExpectedTime()
{
    var actual = DateTime.UtcNow;
    var expected = actual.AddSeconds(-2);

    actual.ShouldBeCloseTo(expected, seconds: 5);
}
```

---

## ✅ ShouldBeAfter

```csharp
[Fact]
public void CreatedAt_ShouldBeAfter_MinThreshold()
{
    var createdAt = DateTime.UtcNow;
    var threshold = createdAt.AddMinutes(-1);

    createdAt.ShouldBeAfter(threshold);
}
```

---

## ✅ ShouldBeBefore

```csharp
[Fact]
public void ExpiryDate_ShouldBeBefore_Cutoff()
{
    var expiry = new DateTime(2025, 01, 01);
    var cutoff = new DateTime(2025, 12, 31);

    expiry.ShouldBeBefore(cutoff);
}
```

---

## ✅ ShouldBeBetweenInclusive

```csharp
[Fact]
public void DeliveryDate_ShouldBeWithinRange()
{
    var delivery = new DateTime(2025, 06, 15);
    var start = new DateTime(2025, 06, 01);
    var end = new DateTime(2025, 06, 30);

    delivery.ShouldBeBetweenInclusive(start, end);
}
```

---

## ✅ Summary

Use these extensions when validating:

- Timestamps from services
- Expiry/renewal windows
- Delivery deadlines or constraints
- Any logic involving `DateTime` values

➡️ Improves test clarity and makes assertions on time more readable and consistent.
