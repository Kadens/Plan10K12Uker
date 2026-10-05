# CLAUDE.md

Guidance for Claude Code when working in this repository.

## What this project is

Plan10K12Uker is a 12-week training and nutrition plan (running, cycling/Zwift, strength) for autumn/winter. Goals: weight loss, higher VO2max, higher FTP, better metabolic health.

Content is written in **Norwegian** (bokmål). Keep user-facing text, wiki pages and UI labels in Norwegian; code, identifiers, commit messages and JSON keys are in English.

## Current repository contents

| Path | Purpose |
|------|---------|
| `wiki/*.md` | Source for the GitHub Wiki. Published by `.github/workflows/publish-wiki.yml` on push to `main` (or manual run). Page links use `./Page-Name` without `.md`. |
| `index.md` | GitHub Pages front page (Jekyll front matter) with the full plan on one page. |
| `data/plan10k12uker.json` | Machine-readable plan: 3 blocks × 4 weeks, weekly `schedule` keyed by English weekday, plus `nutrition`. This is the seed data for the app. |
| `docs/` | Repo structure and wiki link list. Update `docs/STRUCTURE.md` when adding top-level folders. |

When plan content changes, keep `wiki/`, `index.md` and `data/plan10k12uker.json` in sync.

## Planned application

A web app for following the plan and logging training (based on `wiki/Treningsdagbok-mal.md` and `wiki/Progressjonslogg.md`).

| Layer | Technology | Skill |
|-------|------------|-------|
| Frontend | React + TypeScript + Vite | `/react-webapp` |
| Backend | ASP.NET Core on .NET 10 (minimal APIs) | `/dotnet-api` |
| Database | PostgreSQL via EF Core 10 + Npgsql | `/postgresql-db` |
| Hosting | Azure App Service (Linux) + Azure Database for PostgreSQL Flexible Server, Bicep + GitHub Actions | `/azure-deploy` |

Target layout (create as needed):

```
Plan10K12Uker.slnx
src/
  Plan10K12Uker.Api/         # ASP.NET Core API, also serves the built SPA from wwwroot in production
  web/                       # React app (Vite)
tests/
  Plan10K12Uker.Api.Tests/   # xUnit + Testcontainers (PostgreSQL)
infra/
  main.bicep                 # Azure resources
docker-compose.yml           # Local PostgreSQL
.github/workflows/
  publish-wiki.yml
  deploy-app.yml
```

## Commands

```bash
# Local database
docker compose up -d db

# Backend (http://localhost:5080)
dotnet run --project src/Plan10K12Uker.Api
dotnet test

# Frontend (http://localhost:5173, proxies /api to the backend)
cd src/web && npm install && npm run dev
npm run build && npm run lint && npm test
```

## Conventions

- API routes are under `/api`. The frontend only calls relative `/api/...` URLs (Vite proxy in dev, same origin in production — no CORS).
- Secrets never go in the repo. Local: `dotnet user-secrets`. Azure: App Service settings / Key Vault references. GitHub Actions authenticates to Azure with OIDC (no client secrets).
- EF Core migrations are committed under `src/Plan10K12Uker.Api/Data/Migrations`.
- Run `dotnet build`, `dotnet test`, `npm run lint` and `npm run build` before committing app changes.
