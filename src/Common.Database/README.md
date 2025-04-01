# 🔢 Common.Database

This module contains patterns and helpers related to database access.

### Includes:

- `ISqlConnectionFactory` / `SqlConnectionFactory`
- `IRepository<T>` and optionally `RepositoryBase<T>`

### Usage

Register with dependency injection:

```csharp
services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

```
