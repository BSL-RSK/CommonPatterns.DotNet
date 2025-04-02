# 🧩 ObjectComparisonExtensions Examples

This guide demonstrates how to use custom assertion extensions to compare objects while ignoring properties that may vary in tests.

Useful when dealing with:

- Auto-generated fields (IDs, timestamps)
- System metadata (audit info)
- Mapping tests between models (DTO ↔ Entity)

---

## ✅ ShouldBeEquivalentExcluding

### Simple use with single property exclusion:

```csharp
public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
}

[Fact]
public void Users_ShouldMatch_IgnoringId()
{
    var expected = new User { Id = "1", Name = "Alice", CreatedOn = DateTime.UtcNow };
    var actual = new User { Id = "99", Name = "Alice", CreatedOn = expected.CreatedOn };

    actual.ShouldBeEquivalentExcluding(expected, nameof(User.Id));
}
```

---

### 🔁 Excluding multiple fields

```csharp
[Fact]
public void Users_ShouldMatch_IgnoringMultipleProps()
{
    var expected = new User { Id = "1", Name = "Bob", CreatedOn = DateTime.UtcNow };
    var actual = new User { Id = "abc-123", Name = "Bob", CreatedOn = DateTime.UtcNow.AddSeconds(2) };

    actual.ShouldBeEquivalentExcluding(expected, nameof(User.Id), nameof(User.CreatedOn));
}
```

---

## 🧠 Why use this?

- Makes DTO vs Entity comparisons easier
- Avoids unnecessary test breakage due to volatile fields
- Keeps test code DRY and expressive

---

## ✅ Summary

Use `ShouldBeEquivalentExcluding(...)` when you:

- Expect object equivalence _except_ for a few irrelevant fields
- Are comparing domain models vs view models
- Need clean, focused object comparisons in mapping, query, or controller result tests

➡️ This pattern reduces noise in your test comparisons and keeps intent clear.
