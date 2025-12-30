---
workbench:
  type: adr
  workItems:
    - TASK-0005
    - TASK-0006
    - TASK-0007
  codeRefs: []
---

# Terminal UI mode in CLI executable

## Status
Accepted

## Context
Workbench uses Markdown as its primary data model with a CLI for manipulation. Users
want a more discoverable interface that still preserves a single executable and the
existing CLI workflows. The UI must show which CLI command was invoked and support
a global dry-run mode that is clearly indicated in outputs.

## Decision
Implement a terminal UI mode as a `workbench tui` subcommand using Terminal.Gui.
Refactor shared logic into a core library so both CLI and TUI reuse the same parsing,
validation, and command execution paths. Publish as a single-file executable that
contains CLI and TUI projects. The TUI must surface the last command invoked and
provide a global dry-run toggle that labels outputs accordingly.

## Consequences
- Pros: improved discoverability; single executable; shared logic avoids drift.
- Cons: added dependency and build complexity; larger binary; UI testing effort.
