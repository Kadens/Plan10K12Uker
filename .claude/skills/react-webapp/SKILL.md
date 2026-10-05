---
name: react-webapp
description: Build or change the Plan10K12Uker React frontend (React + TypeScript + Vite in src/web). Use when creating the web app, adding pages/components, calling the /api backend, or setting up frontend tests and linting.
---

# React web app (src/web)

## Stack

- React 19 + TypeScript (strict) + Vite
- React Router for pages
- TanStack Query for server state; no global state library unless clearly needed
- Vitest + React Testing Library for tests, ESLint for linting
- Plain CSS modules (no UI framework unless the user asks for one)

## Scaffold (only if `src/web` does not exist)

```bash
npm create vite@latest src/web -- --template react-ts
cd src/web
npm install
npm install react-router @tanstack/react-query
npm install -D vitest @testing-library/react @testing-library/jest-dom jsdom
```

Add to `package.json` scripts: `"test": "vitest run"`.

## Vite config

Proxy the API in development and build into the API's `wwwroot` so the .NET app serves the SPA in production:

```ts
// src/web/vite.config.ts
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: { '/api': 'http://localhost:5080' },
  },
  build: {
    outDir: '../Plan10K12Uker.Api/wwwroot',
    emptyOutDir: true,
  },
  test: { environment: 'jsdom' },
})
```

Add `src/Plan10K12Uker.Api/wwwroot/` to `.gitignore` — it is a build output.

## Structure

```
src/web/src/
  api/          # typed fetch functions + TanStack Query hooks, one file per resource
  components/   # reusable UI
  pages/        # route components (PlanPage, WeekPage, LogPage, ProgressPage)
  types/        # TypeScript types mirroring API DTOs
  App.tsx       # router + QueryClientProvider
```

## Rules

- Call only relative URLs (`/api/plan`, `/api/logs`). Never hard-code hosts.
- Keep API types in `types/` in sync with the C# DTOs in `src/Plan10K12Uker.Api`.
- UI text in Norwegian (e.g. "Treningsdagbok", "Progresjon", "Uke 1–12"); code in English.
- Handle loading and error states for every query.
- Forms for the training log follow the fields in `wiki/Treningsdagbok-mal.md` (type, varighet, intensitet, puls, watt, følelse 1–10, kommentarer, søvn, kosthold).

## Verify

```bash
cd src/web
npm run lint
npm test
npm run build
```

Then run the backend and `npm run dev`, and check the page in a browser.
