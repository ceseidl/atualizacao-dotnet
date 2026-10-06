English | [Português](README.pt-BR.md)

# atualizacao-dotnet

> **Quick start**

```bash
dotnet build
dotnet run --project src/GuiaConsole --no-build
dotnet run --project src/GuiaApi --no-build -- --demo
```

Needs only the .NET 10 SDK. Details in [How to run](#how-to-run).

Companion code for the article "Guia de Atualização Técnica .NET 10 e C# 14". Each snippet covers one study track: C# fundamentals, .NET configuration and DI, ASP.NET Core, data access, security and AI.

## What it is

- **GuiaConsole**: console app with
  - records, primary constructors, collection expressions, the C# 14 `field` keyword, extension members and null-conditional assignment;
  - Options pattern with validation on startup and dependency injection;
  - EF Core 10 (`LeftJoin`, `ExecuteUpdateAsync`) on in-memory SQLite, plus Dapper on the same connection;
  - `IChatClient` from Microsoft.Extensions.AI with a fake client (no network, no keys).
- **GuiaApi**: ASP.NET Core minimal API with built-in OpenAPI, built-in validation (`AddValidation`), JWT bearer authentication and an authorization policy. With `--demo` it starts itself, calls its own endpoints and prints the status codes (401, 201, 400, 200, 403, 204).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

No external services: SQLite runs in memory and the JWT key in `Demo.cs` is a test value (use user-secrets or a key vault in production).

## How to run

From the repository root:

```bash
dotnet build
dotnet run --project src/GuiaConsole --no-build
dotnet run --project src/GuiaApi --no-build -- --demo
```

Expected output of `GuiaApi --demo`:

```
Sem token          401
POST valido        201
POST invalido      400
GET existente      200
DELETE vendedor    403
DELETE gerente     204
```

`GuiaConsole` prints four numbered sections (C# 14, configuration and DI, EF Core and Dapper, AI).

## Structure

```
atualizacao-dotnet.slnx
src/
  GuiaConsole/   Fundamentos.cs, Configuracao.cs, Dados.cs, IA.cs, Program.cs
  GuiaApi/       Program.cs, Pedidos.cs, Demo.cs
```

Identifiers are kept in Portuguese, matching the article.
