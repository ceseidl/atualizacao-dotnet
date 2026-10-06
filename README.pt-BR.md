[English](README.md) | Português

# atualizacao-dotnet

> **Início rápido**

```bash
dotnet build
dotnet run --project src/GuiaConsole --no-build
dotnet run --project src/GuiaApi --no-build -- --demo
```

Precisa apenas do SDK do .NET 10. Detalhes em [Como executar](#como-executar).

Código de apoio do artigo "Guia de Atualização Técnica .NET 10 e C# 14". Cada trecho cobre uma trilha de estudo: fundamentos de C#, configuração e DI no .NET, ASP.NET Core, acesso a dados, segurança e IA.

## O que é

- **GuiaConsole**: aplicação de console com
  - records, construtores primários, collection expressions, a palavra-chave `field` do C# 14, membros de extensão e atribuição condicional nula;
  - Options pattern com validação na inicialização e injeção de dependência;
  - EF Core 10 (`LeftJoin`, `ExecuteUpdateAsync`) em SQLite na memória, mais Dapper na mesma conexão;
  - `IChatClient` do Microsoft.Extensions.AI com um cliente falso (sem rede, sem chaves).
- **GuiaApi**: minimal API do ASP.NET Core com OpenAPI nativo, validação nativa (`AddValidation`), autenticação JWT bearer e uma política de autorização. Com `--demo` ela sobe sozinha, chama os próprios endpoints e imprime os códigos de status (401, 201, 400, 200, 403, 204).

## Pré-requisitos

- [SDK do .NET 10](https://dotnet.microsoft.com/download)

Sem serviços externos: o SQLite roda em memória e a chave JWT de `Demo.cs` é um valor de teste (em produção, use user-secrets ou um cofre de chaves).

## Como executar

Na raiz do repositório:

```bash
dotnet build
dotnet run --project src/GuiaConsole --no-build
dotnet run --project src/GuiaApi --no-build -- --demo
```

Saída esperada de `GuiaApi --demo`:

```
Sem token          401
POST valido        201
POST invalido      400
GET existente      200
DELETE vendedor    403
DELETE gerente     204
```

O `GuiaConsole` imprime quatro seções numeradas (C# 14, configuração e DI, EF Core e Dapper, IA).

## Estrutura

```
atualizacao-dotnet.slnx
src/
  GuiaConsole/   Fundamentos.cs, Configuracao.cs, Dados.cs, IA.cs, Program.cs
  GuiaApi/       Program.cs, Pedidos.cs, Demo.cs
```

Os identificadores ficam em português, como no artigo.
