# Repo-struktur

Plan10K12Uker/
├── README.md
├── CLAUDE.md                 # Instruksjoner for Claude Code
├── .claude/skills/           # Claude Code-skills (react-webapp, dotnet-api, postgresql-db, azure-deploy)
├── Plan10K12Uker.slnx        # .NET-løsning
├── docker-compose.yml        # Lokal PostgreSQL
├── src/
│   └── Plan10K12Uker.Api/    # ASP.NET Core API (.NET 10), EF Core-migrasjoner i Data/Migrations
├── tests/
│   └── Plan10K12Uker.Api.Tests/
├── docs/
│   ├── STRUCTURE.md
│   └── wiki-links.md
├── wiki/                     # Wiki-kilde (synkes via Actions)
│   ├── Home.md
│   ├── Blokk-1.md
│   ├── Blokk-2.md
│   ├── Blokk-3.md
│   ├── Progressjonslogg.md
│   ├── Treningsdagbok-mal.md
│   └── Kalender-12-uker.md
└── data/
    └── plan10k12uker.json
