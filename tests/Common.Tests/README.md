# 🤝 Common.Tests

A shared space for reusable test logic, setup helpers, and standard patterns used across projects.

This folder is designed to:

- Encourage consistency in test structure and naming
- Provide templates for common testing scenarios
- Reduce boilerplate by reusing mocks, builders, and base setups

---

### 📁 Subfolders

- [`TestTemplates`](TestTemplates/README.md)  
  💡 Reusable test case templates for services, repositories, controllers, and validators. Includes examples for real-world usage.

- [`TestUtilities`](TestUtilities/README.md)  
  🛠 Helper classes like `DateTimeStub`, `InMemoryDbHelper`, and test context setup. Includes usage examples.

- [`SharedMocks`](SharedMocks/README.md)  
  🔧 Centralized reusable mocks (e.g. `MockLogger`, `MockUserContext`) for quick setup.

- [`TestData`](TestData/README.md)  
  📦 Sample input data, embedded test files, and `MemberData` providers for data-driven tests.

- [`CustomAssertions`](CustomAssertions/README.md)  
  ✅ Domain-specific or extended assertion helpers to make tests more expressive and readable.

- [`BaseTestClasses`](BaseTestClasses/README.md)  
  🧱 Abstract test bases for common layers like services and controllers. Helpful for shared test setup.

---

### ✅ What's typically included in Common.Tests?

- Test doubles and mocks used across multiple test projects
- Extension methods or utility classes to simplify test writing
- Standardized patterns to improve test readability and maintainability
- Sample test data and helpers for advanced testing scenarios

---

### 📦 Dependencies

This shared test layer assumes use of the following common libraries:

```bash
dotnet add package xunit
dotnet add package Moq
```

Optional (if using FluentValidation or ASP.NET Core test helpers):

```bash
dotnet add package FluentValidation
dotnet add package Microsoft.AspNetCore.Mvc.Core
```

---

> 📌 Tip: Each subfolder contains its own `README.md` and examples to help you understand how and when to use the provided components.
