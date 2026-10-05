---
name: dotnet-api
description: Build or change the Plan10K12Uker backend — ASP.NET Core minimal API on .NET 10 in src/Plan10K12Uker.Api, with xUnit tests. Use when adding endpoints, DTOs, validation, configuration, or setting up the solution.
---

# .NET 10 backend (src/Plan10K12Uker.Api)

## Scaffold (only if the solution does not exist)

```bash
dotnet new sln -n Plan10K12Uker            # creates Plan10K12Uker.slnx
dotnet new web -n Plan10K12Uker.Api -o src/Plan10K12Uker.Api -f net10.0
dotnet new xunit -n Plan10K12Uker.Api.Tests -o tests/Plan10K12Uker.Api.Tests -f net10.0
dotnet sln add src/Plan10K12Uker.Api tests/Plan10K12Uker.Api.Tests
dotnet add tests/Plan10K12Uker.Api.Tests reference src/Plan10K12Uker.Api
dotnet add src/Plan10K12Uker.Api package Microsoft.AspNetCore.OpenApi
dotnet add tests/Plan10K12Uker.Api.Tests package Microsoft.AspNetCore.Mvc.Testing
```

Set the dev URL to `http://localhost:5080` in `Properties/launchSettings.json`. Database setup is in the `/postgresql-db` skill.

## Structure

```
src/Plan10K12Uker.Api/
  Program.cs              # composition root: services, middleware, MapXxxEndpoints()
  Endpoints/              # one static class per resource: PlanEndpoints, LogEndpoints
  Contracts/              # request/response DTOs (records)
  Domain/                 # entities
  Data/                   # AppDbContext, configurations, migrations, seeding
```

## Patterns

- Minimal APIs grouped per resource:

  ```csharp
  public static class LogEndpoints
  {
      public static IEndpointRouteBuilder MapLogEndpoints(this IEndpointRouteBuilder app)
      {
          var group = app.MapGroup("/api/logs").WithTags("Logs");
          group.MapGet("/", GetAll);
          group.MapPost("/", Create);
          return app;
      }

      static async Task<Ok<List<LogDto>>> GetAll(AppDbContext db, CancellationToken ct) => ...
      static async Task<Results<Created<LogDto>, ValidationProblem>> Create(CreateLogRequest req, AppDbContext db, CancellationToken ct) => ...
  }
  ```

- Return `TypedResults` so OpenAPI metadata is accurate.
- DTOs are `record` types; never return EF entities directly.
- Validation: `builder.Services.AddValidation();` (.NET 10 built-in minimal API validation) with DataAnnotations on request records.
- Errors: `builder.Services.AddProblemDetails();` + `app.UseExceptionHandler();` + `app.UseStatusCodePages();`.
- OpenAPI: `builder.Services.AddOpenApi();` and `app.MapOpenApi();` in Development only.
- Health check: `builder.Services.AddHealthChecks()` and `app.MapHealthChecks("/api/health")` — used by Azure.
- Pass `CancellationToken` through to EF Core calls.
- Serve the React build in production:

  ```csharp
  app.UseDefaultFiles();
  app.MapStaticAssets();
  // ... map API endpoints ...
  app.MapFallbackToFile("index.html");
  ```

## Configuration and secrets

- Connection string name: `ConnectionStrings:Postgres`.
- Local secrets: `dotnet user-secrets --project src/Plan10K12Uker.Api set "ConnectionStrings:Postgres" "<value>"`. Never commit real credentials to `appsettings*.json`.

## Tests

- Integration tests with `WebApplicationFactory<Program>` (add `public partial class Program;` at the end of `Program.cs`).
- Use Testcontainers PostgreSQL for database-backed tests (see `/postgresql-db`), not the EF in-memory provider.

## Verify

```bash
dotnet build
dotnet test
dotnet run --project src/Plan10K12Uker.Api   # then GET http://localhost:5080/api/health
```
