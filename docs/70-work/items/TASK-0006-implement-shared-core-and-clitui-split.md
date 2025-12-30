---
id: TASK-0006
type: task
status: draft
priority: medium
owner: platform
created: 2025-12-30
updated: null
githubSynced: null
tags: []
related:
  specs:
    - /docs/10-product/feature-spec-terminal-ui.md
  adrs:
    - /docs/40-decisions/ADR-2025-12-30-terminal-ui-mode-in-cli-executable.md
  files: []
  prs: []
  issues: []
  branches: []
title: Implement shared core and CLI/TUI split
---

# TASK-0006 - Implement shared core and CLI/TUI split

## Summary
Split Workbench into core/CLI/TUI projects while preserving a single published
executable and shared command logic.

## Acceptance criteria
- Core project contains shared parsing, validation, and service logic used by CLI and TUI.
- CLI entrypoint dispatches to CLI or TUI modes without duplicating handlers.
- Publish output remains a single-file executable.
- Spec and ADR links are up to date.
