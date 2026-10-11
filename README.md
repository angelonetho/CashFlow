# CashFlow

A REST API for tracking personal expenses, built with **C# and ASP.NET Core (.NET 8)**.

A study project focused on how a .NET API is organized in layers: each responsibility (HTTP, business rules, domain, data access, contracts and errors) lives in its own project, and dependencies always point inward, toward the domain.

## Stack

- **.NET 8** and **ASP.NET Core Web API**
- **Entity Framework Core** with **MySQL** (Pomelo)
- **FluentValidation** for request validation
- **AutoMapper** to map between DTOs and entities
- **xUnit** and **FluentAssertions** for tests
- **Swagger / OpenAPI** to document and try out the endpoints

## Architecture

```
src/
├── CashFlow.Api              Controllers, exception filter and culture middleware
├── CashFlow.Application      Use cases, validators and mappings (AutoMapper)
├── CashFlow.Domain           Entities, enums and repository interfaces
├── CashFlow.Infrastructure   DbContext, repositories and Unit of Work (EF Core + MySQL)
├── CashFlow.Communication    Request and response contracts (JSON)
└── CashFlow.Exception        Project exceptions and translated error messages
tests/
├── Validators.test           Unit tests for the validators
└── CommonTestUtilities       Test data builders
```

Request flow:

```
HTTP ─► Controller ─► Use case ─► Validator
                         │
                         ▼
                   Repository ─► Unit of Work ─► MySQL
```

- **Use cases** depend only on domain interfaces (`IExpensesRepository`, `IUnitOfWork`). The concrete implementations are registered through dependency injection in the infrastructure layer.
- **Errors** are handled in a single place, the `ExceptionFilter`: validation errors become `400` with the list of messages, and unexpected errors become `500` with a generic message.
- **Error messages** are translated according to the `Accept-Language` header (English by default, Portuguese `pt-BR` and Korean `ko-KR`), resolved by the `CultureMiddleware`.

## Endpoints

| Method | Route | Description | Responses |
|---|---|---|---|
| `POST` | `/api/expenses` | Registers an expense | `201` created · `400` invalid data |
| `GET` | `/api/expenses` | Lists all expenses | `200` with the list · `204` no expenses |

Example request:

```json
POST /api/expenses
Accept-Language: en

{
  "title": "Groceries",
  "description": "Weekly shopping",
  "date": "2024-11-20T10:00:00",
  "amount": 154.90,
  "paymentType": 1
}
```

`paymentType`: `0` cash, `1` credit card, `2` debit card, `3` electronic transfer.

Validation rules:

- `title` is required
- `amount` must be greater than zero
- `date` cannot be in the future
- `paymentType` must be one of the values above

## Running locally

Requirements: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) and a local MySQL 8 server.

1. Create the database and table:

   ```sql
   CREATE DATABASE cashflowdb;
   USE cashflowdb;

   CREATE TABLE Expenses (
     Id          BIGINT AUTO_INCREMENT PRIMARY KEY,
     Title       VARCHAR(255)   NOT NULL,
     Description VARCHAR(2000)  NULL,
     Date        DATETIME       NOT NULL,
     Amount      DECIMAL(10, 2) NOT NULL,
     PaymentType INT            NOT NULL
   );
   ```

2. Set the connection string with .NET *user secrets*, so the password stays out of the repository:

   ```bash
   dotnet user-secrets init --project src/CashFlow.Api
   dotnet user-secrets set "ConnectionStrings:Connection" "Server=localhost;Database=cashflowdb;Uid=root;Pwd=YOUR_PASSWORD;" --project src/CashFlow.Api
   ```

3. Run the API:

   ```bash
   dotnet run --project src/CashFlow.Api
   ```

   In the development environment, Swagger is available at `/swagger`.

4. Run the tests:

   ```bash
   dotnet test
   ```

## Next steps

- [ ] EF Core migrations instead of the manual SQL script
- [ ] SQL Server support
- [ ] Docker Compose with the API and the database
- [ ] GitHub Actions CI pipeline running build and tests
- [ ] Use case tests
- [ ] Endpoints to get by id, update and delete expenses
