# 🧪 CustomAssertions

This folder contains reusable assertion extensions to enhance test clarity, reduce boilerplate, and provide expressive syntax for common testing needs.

All extensions work with [FluentAssertions](https://fluentassertions.com/) and are organized by domain:

---

## 📦 Assertion Categories

### 🔤 [`FluentAssertionExtensions.cs`](./FluentAssertionExtensions.cs)

String and collection helpers for:

- Substring matching
- Type checks and casting
- Non-empty assertions
- Order-insensitive equivalence

📘 [Examples](./FluentAssertionExample.md)

---

### 💥 [`ExceptionAssertionExtensions.cs`](./ExceptionAssertionExtensions.cs)

Test exception types, messages, and inner exceptions:

- Exact or partial message matching
- Inner exception types and messages
- Aggregate exception handling

📘 [Examples](./ExceptionAssertionExample.md)

---

### 🕒 [`DateTimeAssertionExtensions.cs`](./DateTimeAssertionExtensions.cs)

Time-sensitive assertions with clean syntax:

- Close-to (with tolerance)
- Before/After checks
- Range assertions

📘 [Examples](./DateTimeAssertionExample.md)

---

### 🌐 [`HttpResponseAssertionExtensions.cs`](./HttpResponseAssertionExtensions.cs)

Helpers for `HttpResponseMessage` validation:

- Status code and header presence
- Body content matching or empty check

📘 [Examples](./HttpResponseAssertionExample.md)

---

### 🧩 [`ObjectComparisonExtensions.cs`](./ObjectComparisonExtensions.cs)

Compare two objects while ignoring specific fields:

- Ideal for model ↔ DTO comparisons
- Supports multiple exclusions

📘 [Examples](./ObjectAssertionExample.md)

---

### 🔢 [`EnumAssertionExtensions.cs`](./EnumAssertionExtensions.cs)

Assert that enums are valid and values are defined:

- Validate parsed string values
- Ensure numeric enums are within range

📘 [Examples](./EnumAssertionExample.md)

---

## ✅ Usage

To use these extensions:

```bash
dotnet add package FluentAssertions
```

Then simply import the relevant namespace:

```csharp
using Common.Tests.CustomAssertions;
```

Use them anywhere in your test suite to keep assertions:

- ✅ Focused
- ✅ Readable
- ✅ Maintainable

---

> 💡 Tip: Combine multiple assertion categories in integration tests for powerful validation.
