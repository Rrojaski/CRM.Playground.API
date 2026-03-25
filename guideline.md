# AI Development Guideline for CRM.Playground.API

This document is a concise, actionable guideline for the AI assistant working in the `CRM.Playground.API` repository. It explains the project structure, conventions, typical tasks, how to run and test, and safe patterns to follow when modifying code.

---

## Project overview

- Monorepo for FlowSpace-style CRM services. Key projects in this workspace:
  - `CRM.Playground.CRM` — CRM microservice (API + domain + application + infrastructure)
  - `CRM.Playground.Members`, `CRM.Playground.Booking`, `CRM.Playground.Billing`, `CRM.Playground.AI`, `CRM.Playground.Notifications` — other services
  - `CRM.Playground.Tests` — test project (xUnit)

- Target framework: `.NET 8` (TFM `net8.0`).
- Patterns used: Clean Architecture, CQRS with MediatR, EF Core 8 for persistence, FluentValidation for input validation.

---

## Coding & repository conventions

- Keep changes minimal and localized to the files required to implement feature/bugfix.
- Follow existing folder/layout conventions:
  - Domain entities: `CRM.Playground.CRM/CRM.Playground.CRM.Domain/Entities`
  - Application (CQRS/handlers/validators): `CRM.Playground.CRM/CRM.Playground.CRM.Application`
  - Infrastructure (EF Core DbContext): `CRM.Playground.CRM/CRM.Playground.CRM.Infrastructure`
  - API controllers: `CRM.Playground.CRM/Controllers`
  - Tests: `CRM.Playground.Tests`
- Use MediatR for commands/queries and place DTO/Command/Query/Handler classes in `Application`.
- Use FluentValidation classes under `Application/Validators`.
- Use EF Core migrations only for real DB changes; tests should use `Microsoft.EntityFrameworkCore.InMemory`.

---

## How to build and run locally

1. Restore and build solution from repository root:
   - `dotnet restore` then `dotnet build` (solution root `C:\Source\CRM.Playground.API`).
2. Run the CRM API (when configured): `dotnet run --project CRM.Playground.CRM`.
3. App settings are located in `CRM.Playground.CRM/appsettings.json`.

Note: If duplicate assembly attribute errors appear, delete `bin` and `obj` folders for the affected projects and rebuild.

---

## Running tests

- Tests live in `CRM.Playground.Tests` and use xUnit, Moq and EF Core InMemory.
- Run tests from solution root:
  - `dotnet test` or `dotnet test CRM.Playground.Tests/CRM.Playground.Tests.csproj`.

---

## Typical tasks and how to implement them

1. Add new domain entity (e.g., `Contact`):
   - Create entity in `Domain/Entities`.
   - Add DbSet and mapping in `Infrastructure/Persistence/CrmDbContext`.
   - Add Commands/Queries/Handlers in `Application` and validators in `Application/Validators`.
   - Add controller endpoints in `Controllers` that use MediatR.
   - Add tests in `CRM.Playground.Tests` (handlers + controller + validators).

2. Add filtering/search endpoints:
   - Extend `GetAll...Query` with optional filter properties.
   - Implement filter logic in the corresponding handler using IQueryable and apply filters conditionally.
   - Update controller signature to accept query parameters and pass them to MediatR query.
   - Add unit tests for handler and controller.

3. Register new services in `Program.cs` (DI):
   - Register MediatR handlers via `builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(...));`
   - Register DbContext using `builder.Services.AddDbContext<CrmDbContext>(options => options.UseSqlServer(...))`.

---

## Testing guidance

- Prefer unit tests for handlers and validators. Use InMemory DB for handler integration-style tests.
- For controller tests, mock `IMediator` with `Moq` and assert ActionResult types and returned values.
- Keep tests deterministic and independent.

---

## Common pitfalls

- Duplicate assembly attributes: clean `bin`/`obj` folders if build reports duplicate assembly attributes.
- Package version mismatches: prefer EF Core and MediatR packages that match `net8.0`. Add exact versions compatible with .NET 8.
- Do not commit secrets or connection strings; use configuration and secrets management.

---

## CI / PR expectations

- New feature PRs should include unit tests and update documentation where needed.
- Run `dotnet build` and `dotnet test` in CI.
- Keep PRs small and scoped to a single feature/bugfix.

---

If you want, I can now:
- Create a checklist template for new features/PRs, or
- Add example implementation (e.g., `Contact` entity + CQRS + tests), or
- Add Swagger annotations for the CRM endpoints.

Specify which of the above (or another task) to do next.