# 🧱 BaseTestClasses

This folder contains reusable base classes designed to eliminate repetitive setup across common test types.

Unlike `TestTemplates` which provide full test file scaffolds, these base classes serve as **foundation utilities** for controller and service tests.

---

## 🧪 Purpose

- 🔁 Reduce boilerplate setup code
- 📦 Provide pre-configured mocks (e.g., `ILogger<T>`, `HttpContext.User`)
- 🧼 Ensure consistency across your test suite
- ♻️ Designed to be inherited in many test classes

---

## 📂 Included Base Classes

### 🧪 [`ControllerTestBase.cs`](./ControllerTestBase.cs)

Mocked `HttpContext` with user claims for controller tests.

📘 [See example](./ControllerTestBaseExample.md)

---

### ⚙️ [`ServiceTestBase.cs`](./ServiceTestBase.cs)

Provides pre-wired `ILogger<T>` and setup hook for any service under test.

📘 [See example](./ServiceTestBaseExample.md)

---

## ✅ When to Use

Use `BaseTestClasses` when you:

- Want to centralize your controller or service test setup
- Have recurring mocking logic in multiple test files
- Need a shared foundation without duplicating code

---

> 💡 Tip: Use these base classes in combination with your `TestTemplates` for full coverage and efficiency in your testing strategy.
