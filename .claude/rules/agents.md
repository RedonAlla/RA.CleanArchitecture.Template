---
alwaysApply: true
description: This file provides guidance to AI coding agents (Claude Code, Codex, OpenCode, and others) when working in this repository. It is the canonical file; `CLAUDE.md` is a symlink to it.
color: red
---

# Agent

## Model Selection

- **DO** use Sonnet for routine tasks: formatting, simple refactors, test generation, boilerplate.
  Rationale: Sonnet is faster and cheaper for well-defined, low-ambiguity work.

- **DO** use Opus for complex architecture decisions, design reviews, and multi-system analysis.
  Rationale: Opus handles nuance, trade-offs, and large context better for high-stakes decisions.

- **DO** escalate to Fable for the highest-stakes work: greenfield architecture for long-lived systems, or problems that resisted an Opus pass.
  Rationale: Fable is the frontier tier above Opus. Reserve it for decisions where a mistake is very expensive.

- **DO** use model aliases (`fable`, `opus`, `sonnet`, `haiku`) in agent frontmatter and configs — never pinned version IDs.
  Rationale: Aliases track the latest version of each tier; pinned IDs rot when new versions ship.

## Skill Loading

- **DO** load relevant skills before starting work. Check AGENTS.md skill maps for the current task domain.
  Rationale: Skills carry opinionated patterns and anti-patterns that prevent common mistakes.

- **DON'T** start implementation without checking if a relevant skill exists.
  Rationale: Re-discovering best practices wastes time when they are already codified.