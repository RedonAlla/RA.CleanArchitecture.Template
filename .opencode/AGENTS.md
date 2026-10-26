# OpenCode Configuration

OpenCode reads the root `AGENTS.md` natively. This file is loaded via the `instructions` array in `opencode.json` and surfaces the kit's agents, skills, MCP tools, and rules so OpenCode can route work the same way Claude Code does.

## Specialist Agents

Each agent below maps to a focused area of .NET work. The full agent definition (role, skills, boundaries) lives in `agents/<name>.md` and can be loaded directly or adapted into an OpenCode subagent under `.opencode/agents/`.

| Agent | File | When to Use |
|-------|------|-------------|
| dotnet-architect | `agents/dotnet-architect.md` | Architecture decisions, project structure, module boundaries, feature scaffolding |
| api-designer | `agents/api-designer.md` | Minimal API design, OpenAPI specs, versioning, rate limiting, CORS |
| ef-core-specialist | `agents/ef-core-specialist.md` | Database design, EF Core queries, migrations, DbContext configuration |
| test-engineer | `agents/test-engineer.md` | Test strategy, xUnit v3, WebApplicationFactory, Testcontainers |
| security-auditor | `agents/security-auditor.md` | Auth systems, OWASP compliance, secrets management, vulnerability review |
| performance-analyst | `agents/performance-analyst.md` | Profiling, benchmarks, caching strategy, async pattern optimization |
<!-- | devops-engineer | `agents/devops-engineer.md` | Docker, CI/CD pipelines, .NET Aspire orchestration, deployment | -->
| code-reviewer | `agents/code-reviewer.md` | Multi-dimensional code review, PR review, quality gatekeeper |
| build-error-resolver | `agents/build-error-resolver.md` | Autonomous build error fixing, iterative compilation repair |
| refactor-cleaner | `agents/refactor-cleaner.md` | Dead code removal, systematic cleanup, safe refactoring |

## Skills

Skills live in `skills/<skill-name>/SKILL.md` and follow the Agent Skills open standard. Reference them by path when you need the opinionated pattern for a topic.

### .NET Domain Skills
api-versioning, authentication, caching, clean-architecture, configuration, container-publish, dependency-injection, ef-core, error-handling, logging, messaging, minimal-api, modern-csharp, openapi, project-structure, resilience, scalar, serilog, testing, 

### Workflow Skills (each carries its methodology inline)
arch-check, build-fix, checkpoint, code-review, de-sloppify, dotnet-init, health-check, migrate, outdated, plan, scaffold, security-scan, spec, tdd, verify, wrap-up

### Workflow & Learning Skills
convention-learner, workflow-mastery, instinct-system

> Workflow skills map naturally onto OpenCode [custom commands](https://opencode.ai/docs/commands/) (`.opencode/commands/<name>.md`). Create thin command wrappers that point at the matching skill's methodology when you want `/verify`, `/scaffold`, `/tdd`, etc.

## Rules

Always-applied coding conventions are consolidated for OpenCode in `.cursor/rules/dotnet-rules.md` (loaded via `opencode.json`). The canonical per-topic source lives in `.claude/rules/`:

- `coding-style.md` — C# 14 style, naming, file organization
- `architecture.md` — Dependency direction, feature folders, data access
- `security.md` — Secrets, input validation, auth, transport security
- `testing.md` — Integration-first, xUnit v3, Testcontainers, AAA
- `performance.md` — CancellationToken, TimeProvider, caching, async
- `error-handling.md` — Result pattern, ProblemDetails, exception boundaries
- `git-workflow.md` — Conventional commits, atomic commits, branch safety
- `agents.md` — MCP-first tools, subagent routing, skill loading
- `hooks.md` — Format hooks, pre-commit, post-test analysis
- `packages.md` — Latest stable NuGet versions, central package management