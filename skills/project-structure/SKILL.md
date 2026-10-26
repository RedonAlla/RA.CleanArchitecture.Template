---
name: project-structure
description: >
  .NET solution and project structure conventions. Covers .slnx format,
  Directory.Build.props, Directory.Packages.props for central package management,
  global usings, and naming conventions.
  Load this skill when setting up a new solution, adding projects, configuring
  build properties, or when the user mentions "solution structure", ".slnx",
  "Directory.Build.props", "central package management", "Directory.Packages.props",
  "global usings", ".editorconfig", "project layout", or "naming conventions".
---

# Project Structure

## Core Principles

1. **Central package management** — Use `Directory.Packages.props` to manage NuGet package versions in one place. No version numbers in individual `.csproj` files.
2. **Shared build properties** — Use `Directory.Build.props` for common settings (target framework, nullable, implicit usings). Don't repeat in every project.
3. **.slnx for solutions** — The new XML-based solution format is cleaner and more merge-friendly than the legacy `.sln` format.
4. **src/tests separation** — Source projects in `src/`, test projects in `tests/`. Clear boundary.

## Patterns

### Solution Layout

```
RaTemplate/
├── RaTemplate.sln                     # solution folders: Solution Items, src/{Core,Infrastructure,Presentation}, tests
├── global.json                       # SDK 10.0.400, rollForward latestFeature
├── Directory.Build.props             # net10.0, nullable, analysis, SonarAnalyzer for every project
├── Directory.Packages.props          # Central Package Management — all versions live here
├── .editorconfig                     # code style + analyzer rule tuning
├── AGENTS.md / CLAUDE.md             # agent routing and repo guidance
├── README.md                         # human-facing quickstart
├── .agents/skills/, .claude/         # agent skills and rules (e.g. new-entity)
├── src/
│   ├── Core/
│   │   ├── RaTemplate.Domain/
│   │   │   ├── AssemblyReference.cs
│   │   │   ├── Constants/
│   │   │   │   ├── EntitiesConstraints.cs
│   │   │   │   └── ErrorCodes.cs
│   │   │   └── Entities/
│   │   │       └── Event.cs
│   │   └── RaTemplate.Application/
│   │       ├── AssemblyReference.cs
│   │       ├── DependencyInjection.cs
│   │       └── Abstractions/Data/
│   │           └── IRaTemplateSqliteDbContext.cs
│   ├── Infrastructure/
│   │   ├── RaTemplate.Infrastructure/
│   │   │   ├── AssemblyReference.cs
│   │   │   └── DependencyInjection.cs
│   │   ├── RaTemplate.Persistence/
│   │   │   ├── AssemblyReference.cs
│   │   │   ├── DependencyInjection.cs
│   │   │   └── Database/
│   │   │       ├── RaTemplateSqliteDbContext.cs
│   │   │       └── RaTemplateDbInitializer.cs
│   │   └── RaTemplate.Integration/
│   │       ├── AssemblyReference.cs
│   │       └── DependencyInjection.cs
│   └── Presentation/
│       ├── RaTemplate.Api.Contracts/
│       │   └── AssemblyReference.cs
│       └── RaTemplate.Api/
│           ├── Program.cs            # composition root
│           ├── AssemblyReference.cs
│           ├── ServiceConfiguration/
│           │   ├── CorsExtensions.cs
│           │   ├── HealthCheckExtensions.cs
│           │   ├── MiddlewareExtensions.cs
│           │   └── OpenApiExtensions.cs
│           ├── Models/               # template demo models (Pizza, ValueObjects)
│           ├── Properties/launchSettings.json
│           ├── appsettings.json / appsettings.Development.json
│           ├── RaTemplate.Api.http
│           └── RaTemplate.db          # SQLite database (currently tracked in git)
└── tests/
    └── RaTemplate.ArchitectureTests/
        ├── Assemblies.cs, Namespaces.cs   # reflection roots for NetArchTest
        ├── DependencyTests.cs             # layer isolation
        ├── ApplicationTests.cs, HandlersTests.cs   # feature naming + sealing
        ├── EntitiesTests.cs               # entity conventions
        ├── PersistenceTests.cs            # EF configuration conventions
        └── ApiTests.cs                    # endpoint conventions
```

### Directory.Build.props

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>14</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  </PropertyGroup>
</Project>
```

### Directory.Packages.props (Central Package Management)

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>

  <ItemGroup>
    <!-- Versions below are illustrative — resolve the current stable versions
         with `dotnet add package <name>` (no --version flag); see the packages rule -->
    <!-- ASP.NET Core -->
    <PackageVersion Include="Mediator.Abstractions" Version="3.0.0" />
    <PackageVersion Include="Mediator.SourceGenerator" Version="3.0.0" />
    <PackageVersion Include="FluentValidation.DependencyInjectionExtensions" Version="12.0.0" />

    <!-- Data -->
    <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="10.0.10" />
    <PackageVersion Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.10" />

    <!-- Observability -->
    <PackageVersion Include="Serilog.AspNetCore" Version="10.0.0" />
    <PackageVersion Include="OpenTelemetry.Extensions.Hosting" Version="1.17.0" />

    <!-- Testing -->
    <PackageVersion Include="xunit.v3" Version="3.2.2" />
    <PackageVersion Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.10" />
    <PackageVersion Include="Testcontainers.PostgreSql" Version="4.13.0" />
  </ItemGroup>
</Project>
```

### Project File (.csproj) with Central Package Management

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <!-- No TargetFramework here — inherited from Directory.Build.props -->

  <ItemGroup>
    <!-- No Version attribute — managed centrally -->
    <PackageReference Include="Mediator.Abstractions" />
    <PackageReference Include="Mediator.SourceGenerator" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" />
    <PackageReference Include="Serilog.AspNetCore" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\MyApp.Domain\MyApp.Domain.csproj" />
    <ProjectReference Include="..\MyApp.Infrastructure\MyApp.Infrastructure.csproj" />
  </ItemGroup>
</Project>
```

### global.json (SDK Pinning)

```json
{
  "sdk": {
    "version": "10.0.100",
    "rollForward": "latestFeature"
  }
}
```

### .slnx Solution Format

```xml
<Solution>
  <Folder Name="/src/">
    <Project Path="src/MyApp.Api/MyApp.Api.csproj" />
    <Project Path="src/MyApp.Domain/MyApp.Domain.csproj" />
    <Project Path="src/MyApp.Infrastructure/MyApp.Infrastructure.csproj" />
  </Folder>
  <Folder Name="/tests/">
    <Project Path="tests/MyApp.Api.Tests/MyApp.Api.Tests.csproj" />
  </Folder>
</Solution>
```

### Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| Solution | `CompanyName.AppName` or `AppName` | `MyApp.slnx` |
| Project | `AppName.Layer` | `MyApp.Api`, `MyApp.Domain` |
| Namespace | Matches folder path | `MyApp.Api.Features.Orders` |
| Feature folder | PascalCase, plural | `Features/Orders/` |
| Test project | `ProjectName.Tests` | `MyApp.Api.Tests` |

## Anti-patterns

### Don't Scatter Package Versions

```xml
<!-- BAD — version in every .csproj, version drift -->
<PackageReference Include="Mediator.Abstractions" Version="2.0.0" />  <!-- in Project A -->
<PackageReference Include="Mediator.Abstractions" Version="3.0.0" />  <!-- in Project B -->

<!-- GOOD — central management, one version -->
<!-- Directory.Packages.props: <PackageVersion Include="Mediator.Abstractions" Version="3.0.0" /> -->
<!-- .csproj: <PackageReference Include="Mediator.Abstractions" /> -->
```

### Don't Repeat Build Properties

```xml
<!-- BAD — same properties in every .csproj -->
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <Nullable>enable</Nullable>
  <ImplicitUsings>enable</ImplicitUsings>
</PropertyGroup>

<!-- GOOD — once in Directory.Build.props, inherited everywhere -->
```

### Don't Mix Source and Test Projects

```
# BAD — tests mixed with source
src/
  MyApp.Api/
  MyApp.Api.Tests/    # test project in src/

# GOOD — clear separation
src/
  MyApp.Api/
tests/
  MyApp.Api.Tests/
```

## Decision Guide

| Scenario | Recommendation |
|----------|---------------|
| New solution | `.slnx` format |
| Package version management | `Directory.Packages.props` (central) |
| Shared build settings | `Directory.Build.props` |
| SDK version pinning | `global.json` |
| Common using directives | Global usings in `Directory.Build.props` |
| Small API (1-2 devs) | Single project (`MyApp.Api`) |
| Medium API (3-5 devs) | 2-3 projects (`Api`, `Domain`, `Infrastructure`) |
| Large / modular app | Module-per-project with shared `Contracts` |