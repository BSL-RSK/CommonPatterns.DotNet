# 🧪 SharedMocks

A collection of reusable mocks and helpers commonly used across unit and integration test suites.

These components reduce boilerplate and make tests easier to write, read, and maintain — especially when mocking common services like `ILogger` or `IHttpContextAccessor`.

---

## 📁 Contents

### [`MockLogger.cs`](./MockLogger.cs)

Create and verify logs with a single helper.

- ✅ Mock `ILogger<T>`
- 🔍 Assert that a specific log message (or level) was written

➡️ See examples: [`MockLoggerExample.md`](./MockLoggerExample.md)

---

### [`MockUserContext.cs`](./MockUserContext.cs)

Simulate authenticated users via `IHttpContextAccessor`.

- 🔐 Configure user ID and roles
- ✅ Inject into services or controllers that read from `HttpContext.User`

➡️ See examples: [`MockUserContextExample.md`](./MockUserContextExample.md)

---

## ✅ Use Cases

Use these shared mocks to:

- Replace repetitive mock setup in test classes
- Simulate common framework services like logging and user context
- Centralize and standardize common test behaviors

These helpers pair well with:

- 🧪 Service, controller, and middleware tests
- 🔁 Reusable template patterns in `TestTemplates`

---

## 💡 Tip

Use shared mocks when a dependency is mocked in **multiple places** across your codebase.
It keeps tests **dry**, expressive, and easier to maintain.
