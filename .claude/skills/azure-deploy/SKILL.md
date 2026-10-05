---
name: azure-deploy
description: Publish the Plan10K12Uker solution (React SPA + .NET 10 API + PostgreSQL) to Azure — Bicep infrastructure in infra/, GitHub Actions deploy with OIDC, App Service on Linux and Azure Database for PostgreSQL Flexible Server. Use when provisioning Azure resources, writing the deploy workflow, or troubleshooting a deployment.
---

# Deploy to Azure

## Architecture

- **One Azure App Service (Linux, .NET 10)** hosts the API and serves the built React app from `wwwroot` — one deployment, same origin, no CORS.
- **Azure Database for PostgreSQL Flexible Server** (version 17, Burstable B1ms) with database `plan10k12uker`.
- App Service has a **system-assigned managed identity**; the connection string is an App Service setting (or a Key Vault reference).
- Resource group: `rg-plan10k12uker`, region `norwayeast` unless the user says otherwise.

## Confirm before acting

Creating Azure resources costs money and deploying is outward-facing. Show the user what will be created/deployed and get a go-ahead before running `az deployment`, `az webapp deploy` or pushing a workflow that deploys.

## Infrastructure (infra/main.bicep)

Resources:

1. `Microsoft.Web/serverfarms` — Linux plan, `B1` SKU, `reserved: true`
2. `Microsoft.Web/sites` — `linuxFxVersion: 'DOTNETCORE|10.0'`, `httpsOnly: true`, `healthCheckPath: '/api/health'`, identity `SystemAssigned`, app setting `ASPNETCORE_ENVIRONMENT=Production`, connection string `Postgres` (type `PostgreSQL`)
3. `Microsoft.DBforPostgreSQL/flexibleServers` — version `17`, SKU `Standard_B1ms` / `Burstable`, storage 32 GB, admin login from parameters
4. `flexibleServers/databases` — `plan10k12uker`
5. `flexibleServers/firewallRules` — `AllowAzureServices` (0.0.0.0–0.0.0.0)

Parameters: `location`, `appName`, `postgresAdminLogin`, `@secure() postgresAdminPassword`. Never put the password in a parameter file in the repo — pass it from a GitHub secret.

```bash
az group create -n rg-plan10k12uker -l norwayeast
az deployment group what-if -g rg-plan10k12uker -f infra/main.bicep -p postgresAdminPassword=<from secret>
az deployment group create -g rg-plan10k12uker -f infra/main.bicep -p postgresAdminPassword=<from secret>
```

Connection string format: `Host=<server>.postgres.database.azure.com;Database=plan10k12uker;Username=<admin>;Password=<pw>;Ssl Mode=Require`.

## GitHub → Azure authentication (one-time setup, OIDC)

1. Create an Entra app registration / user-assigned identity with a federated credential for `repo:Kadens/Plan10K12Uker:ref:refs/heads/main` (and `environment:production` if using environments).
2. Grant it `Contributor` on `rg-plan10k12uker`.
3. Repo secrets: `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`, `POSTGRES_ADMIN_PASSWORD`.

## Workflow (.github/workflows/deploy-app.yml)

Triggers: `push` to `main` on `src/**`, `infra/**`, the workflow file; plus `workflow_dispatch`. `permissions: { id-token: write, contents: read }`.

Jobs:

1. **build**
   - `actions/setup-node@v4` (Node 22, npm cache) → `npm ci && npm run lint && npm test && npm run build` in `src/web` (outputs to the API's `wwwroot`)
   - `actions/setup-dotnet@v4` with `dotnet-version: 10.0.x` → `dotnet test` → `dotnet publish src/Plan10K12Uker.Api -c Release -o publish`
   - `dotnet tool install -g dotnet-ef` → `dotnet ef migrations script --idempotent -o migrate.sql`
   - Upload `publish/` and `migrate.sql` as artifacts
2. **deploy** (needs build, `environment: production`)
   - `azure/login@v2` with the OIDC secrets
   - `azure/arm-deploy@v2` for `infra/main.bicep` (only if infra changed, or always — it is idempotent)
   - Apply `migrate.sql` with `psql` (`sudo apt-get install -y postgresql-client`), temporarily adding the runner IP to the server firewall and removing it afterwards in an `if: always()` step
   - `azure/webapps-deploy@v3` with `package: publish`
   - Smoke test: `curl --fail https://<app>.azurewebsites.net/api/health`

## Troubleshooting

- Logs: `az webapp log tail -g rg-plan10k12uker -n <app>`.
- 500.30 / startup failure: check `linuxFxVersion` is `DOTNETCORE|10.0` and the connection string setting exists.
- Database connection refused: firewall rule `AllowAzureServices` and `Ssl Mode=Require`.
- SPA routes return 404: ensure `MapFallbackToFile("index.html")` and that `wwwroot` was built before `dotnet publish`.
