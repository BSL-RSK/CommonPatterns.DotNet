# 🧪 TestData

A central place to store test-specific data and utilities that power repeatable, clean, and expressive test cases.

This folder is designed to:

- Provide reusable data for parameterized tests (e.g. with `[Theory]`, `[MemberData]`)
- Avoid duplication across test classes
- Organize large or structured data sets separately from test logic

---

## 📁 Contents

### [`example.json`](./example.json)

Sample mock payload for testing serialization, request models, or user-related logic.

- Simulates a valid user
- Contains roles, metadata, and timestamps

---

### [`MemberDataProviders.cs`](./MemberDataProviders.cs)

Reusable `[MemberData]` sets to support data-driven testing.

Includes:

- `BasicValidationCases`: string input + bool result
- `UserRecords`: sample objects to test mapping/conditions

These can be used across any test class by referencing:

```csharp
[Theory]
[MemberData(nameof(MemberDataProviders.BasicValidationCases), MemberType = typeof(MemberDataProviders))]
```

---

## 🧪 Example Usage

See [`TestDataExample.md`](./TestDataExample.md) for real-world examples of:

- Parameterized tests using `[MemberData]`
- Deserializing from `example.json`
- Validating expected logic outcomes in tests

---

## ✅ Best Practices

- Place large or repeatable inputs here instead of hardcoding in `[InlineData]`
- Keep data readable and purpose-driven (e.g. name test sets clearly)
- Store larger structured samples (JSON, CSV, XML) separately for clarity

---

## 💡 Tip

Use this folder as a foundation for building a consistent and scalable testing culture.
