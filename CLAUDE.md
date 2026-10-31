# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Snap.Hutao.Server is the backend for the Hutao (胡桃) Genshin Impact client. It is a .NET 10 solution with three independently deployable ASP.NET Core Web API applications plus three class libraries. Code comments and commit messages are largely written in Chinese.

See `AGENTS.md` at the repo root for additional agent-facing conventions (it is authoritative for controller-doc and copyright rules).

## Build & run

All commands run from the repo root. The solution file is `src/Snap.Hutao.Server/Snap.Hutao.Server.sln`.

```bash
# Build the whole solution
dotnet build src/Snap.Hutao.Server/Snap.Hutao.Server.sln

# Run one of the three web apps (each has its own launchSettings.json)
dotnet run --project src/Snap.Hutao.Server/Snap.Hutao.Server/Snap.Hutao.Server.csproj
dotnet run --project src/Snap.Hutao.Server/Snap.Hutao.Server.API/Snap.Hutao.Server.API.csproj
dotnet run --project src/Snap.Hutao.Server/Snap.Hutao.Server.Metadata/Snap.Hutao.Server.Metadata.csproj
```

- The main `Snap.Hutao.Server` app listens on `http://localhost:5076`; Swagger UI is served at `/doc` (not `/swagger`).
- The docker-compose stack (`src/Snap.Hutao.Server/docker-compose.yml`) runs four services: `homa-server` (5076), `api-server` (5077), `metadata-server` (5078), and `redis`.
- There are **no test projects** in the solution.

### EF Core migrations

Both EF Core contexts (`AppDbContext` and `MetadataDbContext`) live in `Snap.Hutao.Server.Model`. Migrations are generated via design-time factories (`DesignTimeAppDbContextFactory` / `DesignTimeMetadataDbContextFactory`) that read `appsettings.json` from the `Snap.Hutao.Server.Model` directory (this file is gitignored and must exist locally with valid `ConnectionStrings`).

```bash
cd src/Snap.Hutao.Server/Snap.Hutao.Server.Model
dotnet ef migrations add <Name> --context AppDbContext
dotnet ef migrations add <Name> --context MetadataDbContext
```

Migrations are applied at startup only by the main `Snap.Hutao.Server` app (`Program.MigrateDatabase`); the other two apps only check connectivity.

## Architecture

### Solution layout

- `Snap.Hutao.Server` — the main API host. Handles Passport (auth/registration/OAuth), GachaLog upload/statistics, Spiral Abyss (深渊) and Role Combat (剧演) statistics, Redeem codes, Distribution, Announcements, Afdian webhooks, GitHub webhooks, and telemetry/log upload. Also hosts Quartz background jobs and static pages (`wwwroot/redeem.html`).
- `Snap.Hutao.Server.API` — a smaller public API host for client-facing services (patch/update mirror lookup, wallpapers).
- `Snap.Hutao.Server.Metadata` — serves read-only game metadata (avatars, weapons, materials, quests, etc.) backed by `MetadataDbContext`; a Quartz `MetadataRefreshJob` repopulates it. Refresh is triggered by `GET /refresh` guarded by an `X-Metadata-Key` header.
- `Snap.Hutao.Server.Model` — the data/domain layer. Contains EF Core entities, both `DbContext`s, all migrations, and the DTO/response models.
- `Snap.Hutao.Server.Core` — small async primitives (`AsyncLock`, `AsyncKeyedLock`, `AsyncSemaphore`) and helpers.
- `Snap.Hutao.Server.Extension` — extension methods over `DbSet`, collections, numbers, and semaphores.

### Data layer

Two MySQL databases, configured independently:

- `PrimaryMysql8` → `AppDbContext` (business data). Derives from `IdentityDbContext<HutaoUser, IdentityRole<int>, int>`.
- `MetadataMysql8` → `MetadataDbContext` (read-only game metadata, many `Metadata*` entities).

Redis is used for caching/rate-limiting. `AppDbContext` and `MetadataDbContext` are registered with `AddDbContextPool`.

### Cross-cutting

- **Auth**: JWT Bearer via `Microsoft.AspNetCore.Authentication.JwtBearer`. Each of the three apps uses a distinct `ValidIssuer` (`homa.snaphutaorp.org`, `api.snaphutaorp.org`, `metadata.snaphutaorp.org`) but all derive the signing key from `AppOptions.JwtRaw`. Identity Core is used with a relaxed password policy.
- **Response envelope**: controllers return `Response` / `Response<T>` (in `Model/Response`), serialized with a `retcode` + `message` (+ optional `data`) shape. Business error codes are the `ReturnCode` enum; `0` = success. Do not return raw entities directly.
- **Configuration**: the `App` section binds to `AppOptions`; sub-options (Afdian, Smtp, Github, Discord, etc.) live under `App__...`. All `appsettings.json` files and `.env` are gitignored and contain secrets — never commit them.
- **Background jobs**: Quartz scheduled jobs (statistics refresh, record cleanup, Fufu UID ban refresh, metadata refresh) are registered in each app's `Program.cs`.
- **Observability**: Sentry is wired in every app via `UseSentry` + `AddSentryTunneling` (DSN hardcoded in `Program.cs`).

## Conventions

- **StyleCop analyzers are enforced** (with `stylecop.json` and `.editorconfig`). Every C# file must begin with the copyright header:
  ```csharp
  // Copyright (c) DGP Studio. All rights reserved.
  // Licensed under the MIT license.
  ```
  Do not remove or alter existing headers. `.editorconfig` silences many StyleCop rules — don't re-enable them casually.
- **Public controller actions must carry `[SwaggerOperation]`** with `Summary` and `Description` (see `AGENTS.md`). `Description` should be a multiline raw string describing the request/query fields and caller caveats, without leaking internal details. Avoid duplicating XML `<summary>` comments when the attribute already documents the action.
- `using` directives are placed outside the namespace; file-scoped namespaces are preferred.
- New projects split logic the same way: controllers in a `Controller` folder, options in `Option`, services in `Service`, entities/DTOs in the Model project.
