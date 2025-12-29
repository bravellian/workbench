---
workbench:
  type: doc
  workItems: []
  codeRefs: []
owner: platform
status: active
updated: 2025-12-27
---

# Workbench CLI Help (v0.1)

Usage:
```
workbench <command> [options]
```

Global options:
- `--repo <path>`: target repo (defaults to current dir)
- `--format table|json`: output format (default: table)
- `--no-color`: disable colored output
- `--quiet`: suppress non-error output

Config:
- Repo config path: `.workbench/config.json`

Environment overrides:
- `WORKBENCH_REPO`: default repo path (overrides current dir)
- `WORKBENCH_FORMAT`: default format (`table` or `json`)
- `WORKBENCH_NO_COLOR`: set to `1` to disable color
- `WORKBENCH_QUIET`: set to `1` to suppress non-error output

Exit codes:
- `0`: success, no warnings
- `1`: success with warnings (validate/doctor only)
- `2`: command failed due to errors

Common error codes:
- `WB001`: not a git repo / repo not found
- `WB002`: git not installed / not callable
- `WB010`: config parse or schema error
- `WB020`: work item not found by ID
- `WB030`: front matter schema invalid
- `WB040`: validation error (broken link, duplicate ID, etc.)
- `WB050`: git command failed
- `WB060`: GitHub provider failed

Commands:

- `workbench version`
  - Print CLI version.
  - Example: `workbench version`

- `workbench doctor`
  - Check git, config, and expected paths.
  - Includes GitHub provider checks; missing tokens or unauthenticated gh report a warning.
  - Use `--json` for machine-readable output.
  - Example: `workbench doctor`

- `workbench init [--force] [--skip-wizard] [--non-interactive] [--front-matter] [--configure-openai] [--credential-store <local|external|skip>] [--credential-path <path>] [--openai-provider <openai|none>] [--openai-key <key>] [--openai-model <model>]`
  - Guided setup for scaffolding, front matter guidance, and OpenAI configuration.
  - Runs the `run` wizard afterward unless `--skip-wizard` is set.
  - Example: `workbench init --skip-wizard`

- `workbench run`
  - Launch the interactive wizard for common document and work item actions.
  - Example: `workbench run`

- `workbench sync [--items] [--docs] [--nav] [--issues <true|false>] [--import-issues] [--include-done] [--force] [--dry-run] [--prefer <local|github>]`
  - Run the full repo sync (work items, docs/front matter, and navigation) in order. When no step flags are provided, runs all. Use `--import-issues` to scan GitHub for unlinked issues (slower).
  - Example: `workbench sync --dry-run`

- `workbench scaffold [--force]`
  - Create the default folder structure, templates, and config.
  - Example: `workbench scaffold`

- `workbench config show`
  - Print effective config (defaults + repo config + CLI overrides).
  - Example: `workbench config show --format json`
- `workbench config set --path <path> --value "<...>" [--json]`
  - Update a single config value (dot-path).
  - Example: `workbench config set --path github.owner --value "bravellian"`
- `workbench config credentials set --key <KEY> --value "<...>" [--path <path>]`
  - Write or update a credentials.env entry (defaults to `.workbench/credentials.env`).
  - Example: `workbench config credentials set --key WORKBENCH_AI_OPENAI_KEY --value "<...>"`
- `workbench config credentials unset --key <KEY> [--path <path>]`
  - Remove a credentials.env entry.
  - Example: `workbench config credentials unset --key WORKBENCH_AI_OPENAI_KEY`

- `workbench item new --type <bug|task|spike> --title "<...>" [--status <...>] [--priority <...>] [--owner <...>]`
  - Create a new work item in `work/items` using templates and ID allocation.
  - Example: `workbench item new --type task --title "Add promote command"`
- `workbench item generate --prompt "<...>" [--type <bug|task|spike>] [--status <...>] [--priority <...>] [--owner <...>]`
  - Generate a work item with AI from freeform text and create it in `work/items`.
  - Example: `workbench item generate --prompt "Add guardrails to prevent empty summaries"`
- Status values: `draft`, `ready`, `in-progress`, `blocked`, `done`, `dropped`.
- `workbench item import --issue <id|url...> [--type <bug|task|spike>] [--status <...>] [--priority <...>] [--owner <...>]`
  - Import GitHub issues into work items, linking related PRs when available.
  - Example: `workbench item import --issue 42 --issue https://github.com/org/repo/issues/18`
- Status values: `draft`, `ready`, `in-progress`, `blocked`, `done`, `dropped`.

- `workbench item sync [--id <ID...>] [--issue <id|url...>] [--import-issues] [--prefer <local|github>] [--dry-run]`
  - Sync work items with GitHub issues and branches (two-way, no deletes). Branches are only created when listed in `related.branches`. Defaults to pushing local content to GitHub unless `--prefer github` is set for ID-scoped sync. Use `--import-issues` to scan GitHub for unlinked issues (slower). Missing issues are reported as warnings and sync continues.
  - Example: `workbench item sync --dry-run`

- `workbench item list [--type <...>] [--status <...>] [--include-done]`
  - List work items. Use `--include-done` to include `work/done`.
  - Example: `workbench item list --status ready`
- Status values: `draft`, `ready`, `in-progress`, `blocked`, `done`, `dropped`.

- `workbench item show <ID>`
  - Show metadata and resolved path for an item.
  - Example: `workbench item show TASK-0042`

- `workbench item status <ID> <status> [--note "<...>"]`
  - Update status and updated date. Optionally append a note.
  - Example: `workbench item status TASK-0042 in-progress --note "started implementation"`
- Status values: `draft`, `ready`, `in-progress`, `blocked`, `done`, `dropped`.

- `workbench item close <ID> [--move]`
  - Set status to `done`; optionally move the file to `work/done`.
  - Example: `workbench item close TASK-0042 --move`
- `workbench item normalize [--include-done] [--dry-run]`
  - Normalize work item front matter lists (e.g., tags, related lists).
  - Example: `workbench item normalize --include-done`
- `workbench item delete <ID> [--keep-links]`
  - Delete a work item file and remove doc backlinks (unless `--keep-links`).
  - Example: `workbench item delete TASK-0042`

- `workbench item move <ID> --to <path>`
  - Move a work item file and update inbound links to the old path where possible.
  - Example: `workbench item move TASK-0042 --to work/done/TASK-0042-add-promotion-workflow.md`

- `workbench item rename <ID> --title "<...>"`
  - Regenerate slug from title, rename the file, and update inbound links.
  - Example: `workbench item rename TASK-0042 --title "Finalize promotion workflow"`

- `workbench item link <ID> [--spec <path...>] [--adr <path...>] [--file <path...>] [--pr <url...>] [--issue <id...>] [--dry-run]`
  - Add spec/ADR/file/PR/issue links to a work item and update doc backlinks when applicable.
  - Example: `workbench item link TASK-0042 --spec /docs/10-product/payment-flow.md --pr https://github.com/org/repo/pull/12`

- `workbench item unlink <ID> [--spec <path...>] [--adr <path...>] [--file <path...>] [--pr <url...>] [--issue <id...>] [--dry-run]`
  - Remove spec/ADR/file/PR/issue links from a work item and update doc backlinks when applicable.
  - Example: `workbench item unlink TASK-0042 --adr /docs/40-decisions/2025-01-01-audit-logs.md`

- `workbench board regen`
  - Regenerate `work/WORKBOARD.md`.
  - Example: `workbench board regen`

- `workbench doc new --type <spec|adr|doc|runbook|guide> --title "<...>" [--path <...>] [--work-item <ID...>] [--code-ref <ref...>] [--force]`
  - Create a documentation file with Workbench front matter and optional backlinks.
  - Example: `workbench doc new --type spec --title "Payment flow" --work-item TASK-0042`
- `workbench doc delete --path <...> [--keep-links]`
  - Delete a documentation file and remove links from work items (unless `--keep-links`).
  - Example: `workbench doc delete --path docs/10-product/payment-flow.md`

- `workbench doc link --type <spec|adr> --path <...> --work-item <ID...> [--dry-run]`
  - Link a doc to work items.
  - Example: `workbench doc link --type spec --path docs/10-product/access-model.md --work-item TASK-0100`

- `workbench doc unlink --type <spec|adr> --path <...> --work-item <ID...> [--dry-run]`
  - Unlink a doc from work items.
  - Example: `workbench doc unlink --type adr --path docs/40-decisions/2025-01-01-audit-logs.md --work-item TASK-0123`

- `workbench doc sync [--all] [--issues] [--include-done] [--dry-run]`
  - Sync doc/work item backlinks. `--all` adds Workbench front matter to all docs; `--issues` syncs GitHub issue links; `--include-done` includes done/dropped items; `--dry-run` reports changes without writing.
  - Example: `workbench doc sync --all --issues --dry-run`

- `workbench nav sync [--issues <true|false>] [--include-done] [--workboard <true|false>] [--force] [--dry-run]`
  - Sync doc/work item backlinks, update navigation indexes, and regenerate the workboard. Defaults to syncing issue links; set `--issues false` to skip GitHub lookups. `--workboard false` skips workboard regeneration. `--force` rewrites index sections even if they are unchanged.
  - Example: `workbench nav sync --include-done --issues false --workboard false --force`

- `workbench doc summarize [--staged] [--path <path...>] [--dry-run] [--update-index]`
  - Summarize markdown diffs using AI and append `workbench.changeNotes` entries.
  - Example: `workbench doc summarize --staged --update-index`

- `workbench promote --type <...> --title "<...>" [--push] [--start] [--pr] [--base <branch>] [--draft|--no-draft]`
  - Create a work item, branch, and commit in one step; optionally create a PR.
  - Example: `workbench promote --type task --title "Add validate command" --start --pr --draft`

- `workbench github pr create <ID> [--base <branch>] [--draft] [--fill]`
  - Create a GitHub PR via the configured provider and backlink the PR URL.
  - Example: `workbench github pr create TASK-0042 --draft --fill`

- `workbench validate [--strict] [--link-include <path...>] [--link-exclude <path...>] [--skip-doc-schema]`
  - Validate work items, links, and schemas. `--strict` treats warnings as errors. Use `--link-include/--link-exclude` to scope link checks, and `--skip-doc-schema` if doc schema is not available.
  - Example: `workbench validate --strict --link-exclude docs/tabler --skip-doc-schema`

- `workbench verify [--strict]`
  - Alias for `workbench validate`.

Aliases and intent:
- "verify all work" maps to `workbench validate` (use `--strict` for CI).

Deprecated commands:
- `workbench spec new/link/unlink` -> `workbench doc new/link/unlink --type spec`
- `workbench adr new/link/unlink` -> `workbench doc new/link/unlink --type adr`
- `workbench pr create` -> `workbench github pr create`

Dependencies:
- Commands that read or write work items require a git repo.
- `promote` requires git.
- `pr create` and `promote --pr` require a configured GitHub provider (Octokit token or authenticated gh).
