# Workbench Gaps And TODOs

This file tracks missing features, gaps, and decisions to revisit.

## AOT and Trimming
- Replace reflection-based JSON serialization/deserialization with AOT-safe paths.
- Replace or rework YamlDotNet usage to avoid dynamic code requirements.
- Add a documented verification step for AOT publish output (and expected warnings).

## Work Items
- Implement spec/ADR stub generation and auto-linking (roadmap v0.3).
- Add commands to link/unlink specs, ADRs, files, PRs, and issues without manual edits.
- Enforce optional PR description backlink to work item ID (configurable).
- Decide whether to allow additional work item types beyond bug/task/spike.
- Add hooks installer (pre-commit/pre-push) or CI helper for validation.

## Documentation
- Define a doc front matter schema for specs/ADRs (work item backlinks, related files).
- Add doc templates for specs/ADRs with front matter and consistent sections.
- Implement doc creation command(s) (e.g., workbench doc new, workbench spec new).
- Add validation for doc front matter and backlink consistency.
 - Implement doc sync command to reconcile doc/work item backlinks.

## Two-Way Linking
- Decide on front-matter-only vs source-code marker strategy (or hybrid).
- Implement sync command to reconcile doc <-> work item links.
- Decide how to handle multiple work items per doc and multiple docs per work item.
- Define how renames/moves update backlinks across docs and code.

## Docs and CLI
- Update docs to reflect the final linking model and AOT-safe serialization approach.
- Document any new commands, schemas, or config options.
