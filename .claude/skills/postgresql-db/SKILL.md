---
name: postgresql-db
description: Set up and change the Plan10K12Uker PostgreSQL database — EF Core 10 with Npgsql, migrations, seeding from data/plan10k12uker.json, local Docker database, and Testcontainers tests. Use when adding entities, tables, migrations, or queries.
---

# PostgreSQL + EF Core

## Packages

```bash
dotnet add src/Plan10K12Uker.Api package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/Plan10K12Uker.Api package Microsoft.EntityFrameworkCore.Design
dotnet add tests/Plan10K12Uker.Api.Tests package Testcontainers.PostgreSql
dotnet tool install --global dotnet-ef   # once per machine
```

Use package versions matching .NET 10 (EF Core 10.x, Npgsql EF provider 10.x).

## Local database

`docker-compose.yml` at the repo root:

```yaml
services:
  db:
    image: postgres:17
    environment:
      POSTGRES_DB: plan10k12uker
      POSTGRES_USER: plan
      POSTGRES_PASSWORD: plan_local_dev
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
volumes:
  pgdata:
```

```bash
docker compose up -d db
dotnet user-secrets --project src/Plan10K12Uker.Api set "ConnectionStrings:Postgres" "Host=localhost;Database=plan10k12uker;Username=plan;Password=plan_local_dev"
```

## DbContext

```csharp
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
     .UseSnakeCaseNamingConvention());   // package: EFCore.NamingConventions
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
```

- Table and column names in snake_case.
- Entity configuration in `Data/Configurations/*.cs` via `IEntityTypeConfiguration<T>`, applied with `modelBuilder.ApplyConfigurationsFromAssembly(...)`.
- Use `DateOnly` for training dates, `timestamptz` (`DateTimeOffset`/UTC `DateTime`) for timestamps.
- Use `jsonb` only for genuinely unstructured data; model blocks, weeks, sessions and log entries as tables.

## Domain sketch

- `Block` (id, name, focus, weeks) → `PlannedSession` (block, weekday, description)
- `TrainingLog` (date, type, duration, intensity, avg heart rate, watts, feeling 1–10, comments, sleep, nutrition) — fields from `wiki/Treningsdagbok-mal.md`
- `ProgressEntry` (week, weight, FTP, VO2max estimate, 5 km/10 km time) — from `wiki/Progressjonslogg.md`

## Seeding

Seed blocks and planned sessions from `data/plan10k12uker.json` (copy it to the API as content or embed it). Use EF Core's `UseSeeding`/`UseAsyncSeeding` in `AddDbContext` options, and make the seeding idempotent (check whether data exists first).

## Migrations

```bash
dotnet ef migrations add <Name> --project src/Plan10K12Uker.Api --output-dir Data/Migrations
dotnet ef database update --project src/Plan10K12Uker.Api
```

- Commit migrations. Never edit a migration that has already been applied in Azure; add a new one.
- Production: generate an idempotent script in CI and apply it in the deploy workflow (see `/azure-deploy`), not `Database.Migrate()` at startup:

  ```bash
  dotnet ef migrations script --idempotent --project src/Plan10K12Uker.Api -o migrate.sql
  ```

## Tests

Use `Testcontainers.PostgreSql` (`new PostgreSqlBuilder().WithImage("postgres:17").Build()`) in an xUnit fixture, override the connection string in `WebApplicationFactory`, and run migrations against the container. Docker must be running.
