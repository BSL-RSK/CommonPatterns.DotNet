# 🧰 TestUtilities

A collection of reusable testing utilities to simplify setup, mocking, and isolation in unit/integration tests.

These utilities are shared across multiple test projects to reduce boilerplate and ensure consistency.

---

## 📦 Included Helpers

### [`HttpContextHelper`](./HttpContextHelper.cs)

Quickly mock an authenticated `HttpContext` with user ID and roles.

- 🔐 Simulate authenticated users and claims
- 🧪 Inject into controller/middleware tests

➡️ See examples: [`HttpContextExample.md`](./HttpContextExample.md)

---

### [`InMemoryDbHelper`](./InMemoryDbHelper.cs)

Simplifies registering and resetting in-memory EF Core `DbContext` instances.

- 🔌 Swap out real DB with in-memory store
- 🔄 Reset state between tests

➡️ See examples: [`InMemoryDbExample.md`](./InMemoryDbExample.md)

---

### ⏰ DateTime Utilities

Abstraction and test tools to control time in unit tests.

- [`IDateTimeProvider`](./DateTime/IDateTimeProvider.cs): Interface for accessing the clock
- [`SystemDateTimeProvider`](./DateTime/SystemDateTimeProvider.cs): Production implementation
- [`DateTimeStub`](./DateTime/DateTimeStub.cs): Test implementation to freeze/advance time

➡️ See folder: [`DateTime`](./DateTime)  
➡️ Usage examples: [`DateTime/examples.md`](./DateTime/examples.md)

---

## ✅ Summary

Use the tools in this folder to:

- Reduce test setup complexity
- Write faster, deterministic tests
- Avoid test pollution between runs

These patterns work well across controllers, services, and EF-based repositories.
