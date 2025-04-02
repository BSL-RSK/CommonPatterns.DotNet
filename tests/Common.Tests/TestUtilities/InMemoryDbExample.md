# 🧪 InMemoryDbHelper Usage Examples

This file provides examples for using `InMemoryDbHelper` to test Entity Framework Core `DbContext` logic without requiring a real database.

---

## ✅ Registering In-Memory DbContext

```csharp
var services = new ServiceCollection();
services.AddInMemoryDbContext<AppDbContext>();
var provider = services.BuildServiceProvider();
var context = provider.GetRequiredService<AppDbContext>();
```

### Use Named Databases to Avoid Collisions

```csharp
services.AddInMemoryDbContext<AppDbContext>("TestDB-Users");
```

---

## 🔄 Resetting the Database Between Tests

```csharp
var context = provider.GetRequiredService<AppDbContext>();
InMemoryDbHelper.ResetDatabase(context);
```

This ensures a clean state for each test without reinitializing the service provider.

---

## 🧪 Example Test

```csharp
[Fact]
public void CanInsertEntity_UsingInMemoryDb()
{
    // Arrange
    var services = new ServiceCollection();
    services.AddInMemoryDbContext<AppDbContext>("TestDB");
    var provider = services.BuildServiceProvider();
    var context = provider.GetRequiredService<AppDbContext>();
    InMemoryDbHelper.ResetDatabase(context);

    var user = new User { Name = "Alice" };

    // Act
    context.Users.Add(user);
    context.SaveChanges();

    // Assert
    var saved = context.Users.FirstOrDefault();
    Assert.NotNull(saved);
    Assert.Equal("Alice", saved!.Name);
}
```

---

## ✅ Summary

Use `InMemoryDbHelper` to:

- 🔌 Register `DbContext` in tests without real SQL dependencies
- 🔁 Reset database state between tests
- 🔍 Write focused, fast unit tests for EF Core logic

➡️ Best paired with service/repository tests that rely on EF Core.
