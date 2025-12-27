using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Workbench;

static string ResolveRepo(string? repoArg)
{
    var envRepo = Environment.GetEnvironmentVariable("WORKBENCH_REPO");
    var candidate = repoArg ?? envRepo ?? Directory.GetCurrentDirectory();
    var repoRoot = Repository.FindRepoRoot(candidate);
    if (repoRoot is null)
    {
        throw new InvalidOperationException("Not a git repository.");
    }
    return repoRoot;
}

static string ResolveFormat(string formatArg)
{
    var envFormat = Environment.GetEnvironmentVariable("WORKBENCH_FORMAT");
    return string.IsNullOrWhiteSpace(envFormat) ? formatArg : envFormat;
}

static void WriteJson<T>(T payload, JsonTypeInfo<T> typeInfo)
{
    Console.WriteLine(JsonSerializer.Serialize(payload, typeInfo));
}

static WorkItemPayload ItemToPayload(WorkItem item, bool includeBody = false)
{
    return new WorkItemPayload(
        item.Id,
        item.Type,
        item.Status,
        item.Title,
        item.Priority,
        item.Owner,
        item.Created,
        item.Updated,
        item.Tags,
        new RelatedLinksPayload(
            item.Related.Specs,
            item.Related.Adrs,
            item.Related.Files,
            item.Related.Prs,
            item.Related.Issues),
        item.Slug,
        item.Path,
        includeBody ? item.Body : null);
}

static void SetExitCode(int code) => Environment.ExitCode = code;

static string ApplyPattern(string pattern, WorkItem item)
{
    return pattern
        .Replace("{id}", item.Id)
        .Replace("{slug}", item.Slug)
        .Replace("{title}", item.Title);
}

static string CreatePr(
    string repoRoot,
    WorkbenchConfig config,
    WorkItem item,
    string? baseBranch,
    bool draft,
    bool fill)
{
    var prTitle = $"{item.Id}: {item.Title}";
    var prBody = fill ? PullRequestBuilder.BuildBody(item) : $"Work item: /{Path.GetRelativePath(repoRoot, item.Path).Replace('\\', '/')}";
    var isDraft = draft || config.Github.DefaultDraft;
    var prUrl = GithubService.CreatePullRequest(repoRoot, prTitle, prBody, baseBranch ?? config.Git.DefaultBaseBranch, isDraft);
    WorkItemService.AddPrLink(item.Path, prUrl);
    return prUrl;
}

var repoOption = new Option<string?>(
    name: "--repo",
    description: "Target repo (defaults to current dir)");

var formatOption = new Option<string>(
    name: "--format",
    getDefaultValue: () => "table",
    description: "Output format (table|json)");
formatOption.AddCompletions("table", "json");

var noColorOption = new Option<bool>(
    name: "--no-color",
    description: "Disable colored output");

var quietOption = new Option<bool>(
    name: "--quiet",
    description: "Suppress non-error output");

var root = new RootCommand("Bravellian Workbench CLI");
root.AddGlobalOption(repoOption);
root.AddGlobalOption(formatOption);
root.AddGlobalOption(noColorOption);
root.AddGlobalOption(quietOption);

var versionCommand = new Command("version", "Print CLI version.");
versionCommand.SetHandler(() =>
{
    var version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "0.0.0";
    Console.WriteLine(version);
    SetExitCode(0);
});
root.AddCommand(versionCommand);

var doctorCommand = new Command("doctor", "Check git, config, and expected paths.");
doctorCommand.SetHandler((string? repo, string format) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        var schemaErrors = SchemaValidationService.ValidateConfig(repoRoot);
        var configPath = WorkbenchConfig.GetConfigPath(repoRoot);
        var paths = new[]
        {
            config.Paths.DocsRoot,
            config.Paths.WorkRoot,
            config.Paths.ItemsDir,
            config.Paths.TemplatesDir
        };
        var missing = paths.Where(p => !Directory.Exists(Path.Combine(repoRoot, p))).ToList();
        var checks = new List<DoctorCheck>();
        var hasError = configError is not null || schemaErrors.Count > 0;
        var hasWarnings = false;

        try
        {
            var gitResult = GitService.Run(repoRoot, "--version");
            if (gitResult.ExitCode == 0)
            {
                checks.Add(new DoctorCheck(
                    "git",
                    "ok",
                    new DoctorCheckDetails(
                        Version: gitResult.StdOut,
                        Error: null,
                        Reason: null,
                        Path: null,
                        Missing: null,
                        SchemaErrors: null)));
            }
            else
            {
                checks.Add(new DoctorCheck(
                    "git",
                    "warn",
                    new DoctorCheckDetails(
                        Version: null,
                        Error: gitResult.StdErr,
                        Reason: null,
                        Path: null,
                        Missing: null,
                        SchemaErrors: null)));
                hasWarnings = true;
            }
        }
        catch (Exception)
        {
            checks.Add(new DoctorCheck(
                "git",
                "warn",
                new DoctorCheckDetails(
                    Version: null,
                    Error: "git not installed or not on PATH.",
                    Reason: null,
                    Path: null,
                    Missing: null,
                    SchemaErrors: null)));
            hasWarnings = true;
        }

        checks.Add(new DoctorCheck(
            "repo",
            "ok",
            new DoctorCheckDetails(
                Version: null,
                Error: null,
                Reason: null,
                Path: repoRoot,
                Missing: null,
                SchemaErrors: null)));

        if (File.Exists(configPath) && configError is null && schemaErrors.Count == 0)
        {
            checks.Add(new DoctorCheck(
                "config",
                "ok",
                new DoctorCheckDetails(
                    Version: null,
                    Error: null,
                    Reason: null,
                    Path: configPath,
                    Missing: null,
                    SchemaErrors: null)));
        }
        else
        {
            checks.Add(new DoctorCheck(
                "config",
                "warn",
                new DoctorCheckDetails(
                    Version: null,
                    Error: configError,
                    Reason: null,
                    Path: configPath,
                    Missing: null,
                    SchemaErrors: schemaErrors)));
            hasWarnings = true;
        }

        if (missing.Count == 0)
        {
            checks.Add(new DoctorCheck(
                "paths",
                "ok",
                null));
        }
        else
        {
            checks.Add(new DoctorCheck(
                "paths",
                "warn",
                new DoctorCheckDetails(
                    Version: null,
                    Error: null,
                    Reason: null,
                    Path: null,
                    Missing: missing,
                    SchemaErrors: null)));
            hasWarnings = true;
        }

        var ghStatus = GithubService.CheckAuthStatus(repoRoot);
        if (ghStatus.Status == "ok")
        {
            checks.Add(new DoctorCheck(
                "gh",
                "ok",
                new DoctorCheckDetails(
                    Version: ghStatus.Version,
                    Error: null,
                    Reason: null,
                    Path: null,
                    Missing: null,
                    SchemaErrors: null)));
        }
        else
        {
            checks.Add(new DoctorCheck(
                "gh",
                ghStatus.Status,
                new DoctorCheckDetails(
                    Version: null,
                    Error: null,
                    Reason: ghStatus.Reason,
                    Path: null,
                    Missing: null,
                    SchemaErrors: null)));
            hasWarnings = true;
        }

        if (resolvedFormat == "json")
        {
            var payload = new DoctorOutput(
                !hasError,
                new DoctorData(repoRoot, checks));
            WriteJson(payload, WorkbenchJsonContext.Default.DoctorOutput);
        }
        else
        {
            Console.WriteLine($"Repo: {repoRoot}");
            Console.WriteLine("Checks:");
            foreach (var check in checks)
            {
                Console.WriteLine($"- {check}");
            }
        }

        if (hasError)
        {
            SetExitCode(2);
            return;
        }
        SetExitCode(hasWarnings ? 1 : 0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption);
root.AddCommand(doctorCommand);

var scaffoldForceOption = new Option<bool>("--force", "Overwrite existing files.");
var scaffoldCommand = new Command("scaffold", "Create the default folder structure, templates, and config.");
scaffoldCommand.AddOption(scaffoldForceOption);
scaffoldCommand.AddAlias("init");
scaffoldCommand.SetHandler((string? repo, string format, bool force) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var result = ScaffoldService.Scaffold(repoRoot, force);
        if (resolvedFormat == "json")
        {
            var payload = new ScaffoldOutput(
                true,
                new ScaffoldData(result.Created, result.Skipped, result.ConfigPath));
            WriteJson(payload, WorkbenchJsonContext.Default.ScaffoldOutput);
        }
        else
        {
            Console.WriteLine("Scaffold complete.");
            if (result.Created.Count > 0)
            {
                Console.WriteLine("Created:");
                foreach (var path in result.Created)
                {
                    Console.WriteLine($"- {path}");
                }
            }
            if (result.Skipped.Count > 0)
            {
                Console.WriteLine("Skipped:");
                foreach (var path in result.Skipped)
                {
                    Console.WriteLine($"- {path}");
                }
            }
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, scaffoldForceOption);
root.AddCommand(scaffoldCommand);

var configCommand = new Command("config", "Configuration commands.");
var configShowCommand = new Command("show", "Print effective config (defaults + repo config + CLI overrides).");
configShowCommand.SetHandler((string? repo, string format) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (resolvedFormat == "json")
        {
            var payload = new ConfigOutput(
                configError is null,
                new ConfigData(
                    config,
                    new ConfigSources(true, WorkbenchConfig.GetConfigPath(repoRoot))));
            WriteJson(payload, WorkbenchJsonContext.Default.ConfigOutput);
        }
        else
        {
            Console.WriteLine($"Config path: {WorkbenchConfig.GetConfigPath(repoRoot)}");
            if (configError is not null)
            {
                Console.WriteLine($"Config error: {configError}");
            }
            Console.WriteLine(JsonSerializer.Serialize(config, WorkbenchJsonContext.Default.WorkbenchConfig));
        }
        SetExitCode(configError is null ? 0 : 2);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption);
configCommand.AddCommand(configShowCommand);
root.AddCommand(configCommand);

var itemCommand = new Command("item", "Work item commands.");

var itemTypeOption = new Option<string>(
    name: "--type",
    description: "Work item type: bug, task, spike");
itemTypeOption.AddCompletions("bug", "task", "spike");
itemTypeOption.IsRequired = true;

static Option<string> CreateTitleOption()
{
    return new Option<string>(
        name: "--title",
        description: "Work item title")
    {
        IsRequired = true
    };
}

static Option<string?> CreateStatusOption()
{
    var option = new Option<string?>(
        name: "--status",
        description: "Work item status");
    option.AddCompletions("draft", "ready", "in-progress", "blocked", "done", "dropped");
    return option;
}

static Option<string?> CreatePriorityOption()
{
    var option = new Option<string?>(
        name: "--priority",
        description: "Work item priority");
    option.AddCompletions("low", "medium", "high", "critical");
    return option;
}

static Option<string?> CreateOwnerOption()
{
    return new Option<string?>(
        name: "--owner",
        description: "Work item owner");
}

var itemNewCommand = new Command("new", "Create a new work item in work/items using templates and ID allocation.");
var itemTitleOption = CreateTitleOption();
var itemStatusOption = CreateStatusOption();
var itemPriorityOption = CreatePriorityOption();
var itemOwnerOption = CreateOwnerOption();
itemNewCommand.AddOption(itemTypeOption);
itemNewCommand.AddOption(itemTitleOption);
itemNewCommand.AddOption(itemStatusOption);
itemNewCommand.AddOption(itemPriorityOption);
itemNewCommand.AddOption(itemOwnerOption);
itemNewCommand.SetHandler((string? repo, string format, string type, string title, string? status, string? priority, string? owner) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var result = WorkItemService.CreateItem(repoRoot, config, type, title, status, priority, owner);
        if (resolvedFormat == "json")
        {
            var payload = new ItemCreateOutput(
                true,
                new ItemCreateData(result.Id, result.Slug, result.Path));
            WriteJson(payload, WorkbenchJsonContext.Default.ItemCreateOutput);
        }
        else
        {
            Console.WriteLine($"{result.Id} created at {result.Path}");
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, itemTypeOption, itemTitleOption, itemStatusOption, itemPriorityOption, itemOwnerOption);
itemCommand.AddCommand(itemNewCommand);

var itemListCommand = new Command("list", "List work items.");
var listTypeOption = new Option<string>("--type", "Filter by type");
listTypeOption.AddCompletions("bug", "task", "spike");
var listStatusOption = new Option<string>("--status", "Filter by status");
listStatusOption.AddCompletions("draft", "ready", "in-progress", "blocked", "done", "dropped");
var includeDoneOption = new Option<bool>("--include-done", "Include items from work/done.");
itemListCommand.AddOption(listTypeOption);
itemListCommand.AddOption(listStatusOption);
itemListCommand.AddOption(includeDoneOption);
itemListCommand.SetHandler((string? repo, string format, string? type, string? status, bool includeDone) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var list = WorkItemService.ListItems(repoRoot, config, includeDone);
        var items = list.Items;
        if (!string.IsNullOrWhiteSpace(type))
        {
            items = items.Where(item => item.Type.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (!string.IsNullOrWhiteSpace(status))
        {
            items = items.Where(item => item.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (resolvedFormat == "json")
        {
            var payloadItems = items
                .Select(item => new ItemSummary(item.Id, item.Type, item.Status, item.Title, item.Path))
                .ToList();
            var payload = new ItemListOutput(
                true,
                new ItemListData(payloadItems));
            WriteJson(payload, WorkbenchJsonContext.Default.ItemListOutput);
        }
        else
        {
            foreach (var item in items.OrderBy(item => item.Id))
            {
                Console.WriteLine($"{item.Id}\t{item.Status}\t{item.Title}");
            }
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, listTypeOption, listStatusOption, includeDoneOption);
itemCommand.AddCommand(itemListCommand);

var itemShowCommand = new Command("show", "Show metadata and resolved path for an item.");
var itemIdArg = new Argument<string>("id", "Work item ID (e.g., TASK-0042).");
itemShowCommand.AddArgument(itemIdArg);
itemShowCommand.SetHandler((string? repo, string format, string id) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var path = WorkItemService.GetItemPathById(repoRoot, config, id);
        var item = WorkItemService.LoadItem(path) ?? throw new InvalidOperationException("Invalid work item.");
        if (resolvedFormat == "json")
        {
            var payload = new ItemShowOutput(
                true,
                new ItemShowData(ItemToPayload(item)));
            WriteJson(payload, WorkbenchJsonContext.Default.ItemShowOutput);
        }
        else
        {
            Console.WriteLine($"{item.Id} - {item.Title}");
            Console.WriteLine($"Type: {item.Type}");
            Console.WriteLine($"Status: {item.Status}");
            Console.WriteLine($"Path: {item.Path}");
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, itemIdArg);
itemCommand.AddCommand(itemShowCommand);

var itemStatusCommand = new Command("status", "Update status and updated date.");
var statusIdArg = new Argument<string>("id", "Work item ID.");
var statusValueArg = new Argument<string>("status", "New status.");
var noteOption = new Option<string?>("--note", "Append a note.");
itemStatusCommand.AddArgument(statusIdArg);
itemStatusCommand.AddArgument(statusValueArg);
itemStatusCommand.AddOption(noteOption);
itemStatusCommand.SetHandler((string? repo, string format, string id, string status, string? note) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var path = WorkItemService.GetItemPathById(repoRoot, config, id);
        var updated = WorkItemService.UpdateStatus(path, status, note);
        if (resolvedFormat == "json")
        {
            var payload = new ItemStatusOutput(
                true,
                new ItemStatusData(ItemToPayload(updated)));
            WriteJson(payload, WorkbenchJsonContext.Default.ItemStatusOutput);
        }
        else
        {
            Console.WriteLine($"{updated.Id} status updated to {updated.Status}.");
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, statusIdArg, statusValueArg, noteOption);
itemCommand.AddCommand(itemStatusCommand);

var itemCloseCommand = new Command("close", "Set status to done; optionally move to work/done.");
var closeIdArg = new Argument<string>("id", "Work item ID.");
var moveOption = new Option<bool>("--move", "Move to work/done.");
itemCloseCommand.AddArgument(closeIdArg);
itemCloseCommand.AddOption(moveOption);
itemCloseCommand.SetHandler((string? repo, string format, string id, bool move) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var path = WorkItemService.GetItemPathById(repoRoot, config, id);
        var updated = WorkItemService.Close(path, move, config, repoRoot);
        if (move)
        {
            var oldPath = path;
            var newPath = updated.Path;
            if (!string.Equals(oldPath, newPath, StringComparison.OrdinalIgnoreCase))
            {
                LinkUpdater.UpdateLinks(repoRoot, oldPath, newPath);
            }
        }
        if (resolvedFormat == "json")
        {
            var payload = new ItemCloseOutput(
                true,
                new ItemCloseData(ItemToPayload(updated), move));
            WriteJson(payload, WorkbenchJsonContext.Default.ItemCloseOutput);
        }
        else
        {
            Console.WriteLine($"{updated.Id} closed.");
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, closeIdArg, moveOption);
itemCommand.AddCommand(itemCloseCommand);

var itemMoveCommand = new Command("move", "Move a work item file and update inbound links where possible.");
var moveIdArg = new Argument<string>("id", "Work item ID.");
var moveToOption = new Option<string>("--to", "Destination path.") { IsRequired = true };
itemMoveCommand.AddArgument(moveIdArg);
itemMoveCommand.AddOption(moveToOption);
itemMoveCommand.SetHandler((string? repo, string format, string id, string to) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var path = WorkItemService.GetItemPathById(repoRoot, config, id);
        var updated = WorkItemService.Move(path, to, repoRoot);
        LinkUpdater.UpdateLinks(repoRoot, path, updated.Path);
        if (resolvedFormat == "json")
        {
            var payload = new ItemMoveOutput(
                true,
                new ItemMoveData(ItemToPayload(updated)));
            WriteJson(payload, WorkbenchJsonContext.Default.ItemMoveOutput);
        }
        else
        {
            Console.WriteLine($"{updated.Id} moved to {updated.Path}.");
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, moveIdArg, moveToOption);
itemCommand.AddCommand(itemMoveCommand);

var itemRenameCommand = new Command("rename", "Regenerate slug from title, rename the file, and update inbound links.");
var renameIdArg = new Argument<string>("id", "Work item ID.");
var renameTitleOption = new Option<string>("--title", "New title.") { IsRequired = true };
itemRenameCommand.AddArgument(renameIdArg);
itemRenameCommand.AddOption(renameTitleOption);
itemRenameCommand.SetHandler((string? repo, string format, string id, string title) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var path = WorkItemService.GetItemPathById(repoRoot, config, id);
        var updated = WorkItemService.Rename(path, title, config, repoRoot);
        LinkUpdater.UpdateLinks(repoRoot, path, updated.Path);
        if (resolvedFormat == "json")
        {
            var payload = new ItemRenameOutput(
                true,
                new ItemRenameData(ItemToPayload(updated)));
            WriteJson(payload, WorkbenchJsonContext.Default.ItemRenameOutput);
        }
        else
        {
            Console.WriteLine($"{updated.Id} renamed to {updated.Path}.");
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, renameIdArg, renameTitleOption);
itemCommand.AddCommand(itemRenameCommand);

root.AddCommand(itemCommand);

var addCommand = new Command("add", "Shorthand for item creation.");
Command BuildAddCommand(string typeName)
{
    var cmd = new Command(typeName, $"Alias for workbench item new --type {typeName}.");
    var titleOption = CreateTitleOption();
    var statusOption = CreateStatusOption();
    var priorityOption = CreatePriorityOption();
    var ownerOption = CreateOwnerOption();
    cmd.AddOption(titleOption);
    cmd.AddOption(statusOption);
    cmd.AddOption(priorityOption);
    cmd.AddOption(ownerOption);
    cmd.SetHandler((string? repo, string format, string title, string? status, string? priority, string? owner) =>
    {
        try
        {
            var repoRoot = ResolveRepo(repo);
            var resolvedFormat = ResolveFormat(format);
            var config = WorkbenchConfig.Load(repoRoot, out var configError);
            if (configError is not null)
            {
                Console.WriteLine($"Config error: {configError}");
                SetExitCode(2);
                return;
            }
            var result = WorkItemService.CreateItem(repoRoot, config, typeName, title, status, priority, owner);
            if (resolvedFormat == "json")
            {
                var payload = new ItemCreateOutput(
                    true,
                    new ItemCreateData(result.Id, result.Slug, result.Path));
                WriteJson(payload, WorkbenchJsonContext.Default.ItemCreateOutput);
            }
            else
            {
                Console.WriteLine($"{result.Id} created at {result.Path}");
            }
            SetExitCode(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            SetExitCode(2);
        }
    }, repoOption, formatOption, titleOption, statusOption, priorityOption, ownerOption);
    return cmd;
}
addCommand.AddCommand(BuildAddCommand("task"));
addCommand.AddCommand(BuildAddCommand("bug"));
addCommand.AddCommand(BuildAddCommand("spike"));
root.AddCommand(addCommand);

var boardCommand = new Command("board", "Workboard commands.");
var boardRegenCommand = new Command("regen", "Regenerate work/WORKBOARD.md.");
boardRegenCommand.SetHandler((string? repo, string format) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var result = WorkboardService.Regenerate(repoRoot, config);
        if (resolvedFormat == "json")
        {
            var payload = new BoardOutput(
                true,
                new BoardData(result.Path, result.Counts));
            WriteJson(payload, WorkbenchJsonContext.Default.BoardOutput);
        }
        else
        {
            Console.WriteLine($"Workboard regenerated: {result.Path}");
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption);
boardCommand.AddCommand(boardRegenCommand);
root.AddCommand(boardCommand);

var promoteCommand = new Command("promote", "Create a work item, branch, and commit in one step.");
var promoteTypeOption = new Option<string>("--type", "Work item type: bug, task, spike") { IsRequired = true };
promoteTypeOption.AddCompletions("bug", "task", "spike");
var promoteTitleOption = new Option<string>("--title", "Work item title") { IsRequired = true };
var promotePushOption = new Option<bool>("--push", "Push the branch to origin.");
var promoteStartOption = new Option<bool>("--start", "Set status to in-progress.");
var promotePrOption = new Option<bool>("--pr", "Create a GitHub PR.");
var promoteBaseOption = new Option<string?>("--base", "Base branch for PR.");
var promoteDraftOption = new Option<bool>("--draft", "Create a draft PR.");
var promoteNoDraftOption = new Option<bool>("--no-draft", "Create a ready PR.");
promoteCommand.AddOption(promoteTypeOption);
promoteCommand.AddOption(promoteTitleOption);
promoteCommand.AddOption(promotePushOption);
promoteCommand.AddOption(promoteStartOption);
promoteCommand.AddOption(promotePrOption);
promoteCommand.AddOption(promoteBaseOption);
promoteCommand.AddOption(promoteDraftOption);
promoteCommand.AddOption(promoteNoDraftOption);
promoteCommand.SetHandler((InvocationContext context) =>
{
    var repo = context.ParseResult.GetValueForOption(repoOption);
    var format = context.ParseResult.GetValueForOption(formatOption) ?? "table";
    var type = context.ParseResult.GetValueForOption(promoteTypeOption) ?? string.Empty;
    var title = context.ParseResult.GetValueForOption(promoteTitleOption) ?? string.Empty;
    var push = context.ParseResult.GetValueForOption(promotePushOption);
    var start = context.ParseResult.GetValueForOption(promoteStartOption);
    var pr = context.ParseResult.GetValueForOption(promotePrOption);
    var baseBranch = context.ParseResult.GetValueForOption(promoteBaseOption);
    var draft = context.ParseResult.GetValueForOption(promoteDraftOption);
    var noDraft = context.ParseResult.GetValueForOption(promoteNoDraftOption);

    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        if (config.Git.RequireCleanWorkingTree && !GitService.IsClean(repoRoot))
        {
            Console.WriteLine("Working tree is not clean.");
            SetExitCode(2);
            return;
        }

        var status = start ? "in-progress" : null;
        var created = WorkItemService.CreateItem(repoRoot, config, type, title, status, null, null);
        var item = WorkItemService.LoadItem(created.Path) ?? throw new InvalidOperationException("Failed to load work item.");

        var branch = ApplyPattern(config.Git.BranchPattern, item);
        GitService.CheckoutNewBranch(repoRoot, branch);
        GitService.Add(repoRoot, created.Path);

        var commitMessage = ApplyPattern(config.Git.CommitMessagePattern, item);
        var sha = GitService.Commit(repoRoot, commitMessage);

        var shouldPush = push || pr;
        if (shouldPush)
        {
            GitService.Push(repoRoot, branch);
        }

        string? prUrl = null;
        if (pr)
        {
            var useDraft = draft || (!noDraft && config.Github.DefaultDraft);
            prUrl = CreatePr(repoRoot, config, item, baseBranch, useDraft, fill: true);
        }

        if (resolvedFormat == "json")
        {
            var payload = new PromoteOutput(
                true,
                new PromoteData(
                    ItemToPayload(item),
                    branch,
                    new CommitInfo(sha, commitMessage),
                    shouldPush,
                    prUrl));
            WriteJson(payload, WorkbenchJsonContext.Default.PromoteOutput);
        }
        else
        {
            Console.WriteLine($"{item.Id} promoted on {branch}.");
            if (prUrl is not null)
            {
                Console.WriteLine($"PR: {prUrl}");
            }
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
});
root.AddCommand(promoteCommand);

var prCommand = new Command("pr", "Pull request commands.");
var prCreateCommand = new Command("create", "Create a GitHub PR via gh and backlink the PR URL.");
var prIdArg = new Argument<string>("id", "Work item ID.");
var prBaseOption = new Option<string?>("--base", "Base branch for PR.");
var prDraftOption = new Option<bool>("--draft", "Create as draft.");
var prFillOption = new Option<bool>("--fill", "Fill PR body from work item.");
prCreateCommand.AddArgument(prIdArg);
prCreateCommand.AddOption(prBaseOption);
prCreateCommand.AddOption(prDraftOption);
prCreateCommand.AddOption(prFillOption);
prCreateCommand.SetHandler((string? repo, string format, string id, string? baseBranch, bool draft, bool fill) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var path = WorkItemService.GetItemPathById(repoRoot, config, id);
        var item = WorkItemService.LoadItem(path) ?? throw new InvalidOperationException("Invalid work item.");
        var prUrl = CreatePr(repoRoot, config, item, baseBranch, draft, fill);

        if (resolvedFormat == "json")
        {
            var payload = new PrOutput(
                true,
                new PrData(prUrl, item.Id));
            WriteJson(payload, WorkbenchJsonContext.Default.PrOutput);
        }
        else
        {
            Console.WriteLine(prUrl);
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, prIdArg, prBaseOption, prDraftOption, prFillOption);
prCommand.AddCommand(prCreateCommand);
root.AddCommand(prCommand);

var createCommand = new Command("create", "Create resources.");
var createPrCommand = new Command("pr", "Alias for workbench pr create.");
createPrCommand.AddArgument(prIdArg);
createPrCommand.AddOption(prBaseOption);
createPrCommand.AddOption(prDraftOption);
createPrCommand.AddOption(prFillOption);
createPrCommand.SetHandler((string? repo, string format, string id, string? baseBranch, bool draft, bool fill) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var path = WorkItemService.GetItemPathById(repoRoot, config, id);
        var item = WorkItemService.LoadItem(path) ?? throw new InvalidOperationException("Invalid work item.");
        var prUrl = CreatePr(repoRoot, config, item, baseBranch, draft, fill);

        if (resolvedFormat == "json")
        {
            var payload = new PrOutput(
                true,
                new PrData(prUrl, item.Id));
            WriteJson(payload, WorkbenchJsonContext.Default.PrOutput);
        }
        else
        {
            Console.WriteLine(prUrl);
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, prIdArg, prBaseOption, prDraftOption, prFillOption);
createCommand.AddCommand(createPrCommand);
root.AddCommand(createCommand);

var docCommand = new Command("doc", "Documentation commands.");

var docNewCommand = new Command("new", "Create a documentation file with Workbench front matter.");
var docTypeOption = new Option<string>("--type", "Doc type: spec, adr, doc, runbook, guide") { IsRequired = true };
docTypeOption.AddCompletions("spec", "adr", "doc", "runbook", "guide");
var docTitleOption = new Option<string>("--title", "Doc title") { IsRequired = true };
var docPathOption = new Option<string?>("--path", "Destination path (defaults by type).");
var docWorkItemOption = new Option<string[]>(
    "--work-item",
    description: "Link one or more work items.")
{
    AllowMultipleArgumentsPerToken = true
};
var docCodeRefOption = new Option<string[]>(
    "--code-ref",
    description: "Add code reference(s) (e.g., src/Foo.cs#L10-L20).")
{
    AllowMultipleArgumentsPerToken = true
};
var docForceOption = new Option<bool>("--force", "Overwrite existing file.");

docNewCommand.AddOption(docTypeOption);
docNewCommand.AddOption(docTitleOption);
docNewCommand.AddOption(docPathOption);
docNewCommand.AddOption(docWorkItemOption);
docNewCommand.AddOption(docCodeRefOption);
docNewCommand.AddOption(docForceOption);
docNewCommand.SetHandler((
    string? repo,
    string format,
    string type,
    string title,
    string? path,
    string[] workItems,
    string[] codeRefs,
    bool force) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }

        var result = DocService.CreateDoc(
            repoRoot,
            config,
            type,
            title,
            path,
            workItems.ToList(),
            codeRefs.ToList(),
            force);

        if (resolvedFormat == "json")
        {
            var payload = new DocCreateOutput(
                true,
                new DocCreateData(result.Path, result.Type, result.WorkItems));
            WriteJson(payload, WorkbenchJsonContext.Default.DocCreateOutput);
        }
        else
        {
            Console.WriteLine($"Doc created at {result.Path}");
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, docTypeOption, docTitleOption, docPathOption, docWorkItemOption, docCodeRefOption, docForceOption);

var docSyncCommand = new Command("sync", "Sync doc/work item backlinks.");
var docSyncAllOption = new Option<bool>("--all", "Add Workbench front matter to all docs.");
var docSyncDryRunOption = new Option<bool>("--dry-run", "Report changes without writing files.");
docSyncCommand.AddOption(docSyncAllOption);
docSyncCommand.AddOption(docSyncDryRunOption);
docSyncCommand.SetHandler((string? repo, string format, bool all, bool dryRun) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }

        var result = DocService.SyncLinks(repoRoot, config, all, dryRun);
        if (resolvedFormat == "json")
        {
            var payload = new DocSyncOutput(
                true,
                new DocSyncData(
                    result.DocsUpdated,
                    result.ItemsUpdated,
                    result.MissingDocs,
                    result.MissingItems));
            WriteJson(payload, WorkbenchJsonContext.Default.DocSyncOutput);
        }
        else
        {
            Console.WriteLine($"Docs updated: {result.DocsUpdated}");
            Console.WriteLine($"Work items updated: {result.ItemsUpdated}");
            if (result.MissingDocs.Count > 0)
            {
                Console.WriteLine("Missing docs:");
                foreach (var entry in result.MissingDocs)
                {
                    Console.WriteLine($"- {entry}");
                }
            }
            if (result.MissingItems.Count > 0)
            {
                Console.WriteLine("Missing work items:");
                foreach (var entry in result.MissingItems)
                {
                    Console.WriteLine($"- {entry}");
                }
            }
        }
        SetExitCode(0);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, docSyncAllOption, docSyncDryRunOption);

docCommand.AddCommand(docNewCommand);
docCommand.AddCommand(docSyncCommand);
root.AddCommand(docCommand);

var validateCommand = new Command("validate", "Validate work items, links, and schemas.");
var strictOption = new Option<bool>("--strict", "Treat warnings as errors.");
var verboseOption = new Option<bool>("--verbose", "Show detailed validation output.");
validateCommand.AddOption(strictOption);
validateCommand.AddOption(verboseOption);
validateCommand.AddAlias("verify");
validateCommand.SetHandler((string? repo, string format, bool strict, bool verbose) =>
{
    try
    {
        var repoRoot = ResolveRepo(repo);
        var resolvedFormat = ResolveFormat(format);
        var config = WorkbenchConfig.Load(repoRoot, out var configError);
        if (configError is not null)
        {
            Console.WriteLine($"Config error: {configError}");
            SetExitCode(2);
            return;
        }
        var result = ValidationService.ValidateRepo(repoRoot, config);
        var exit = result.Errors.Count > 0 ? 2 : result.Warnings.Count > 0 ? (strict ? 2 : 1) : 0;

        if (resolvedFormat == "json")
        {
            var payload = new ValidateOutput(
                result.Errors.Count == 0,
                new ValidateData(
                    result.Errors,
                    result.Warnings,
                    new ValidateCounts(
                        result.Errors.Count,
                        result.Warnings.Count,
                        result.WorkItemCount,
                        result.MarkdownFileCount)));
            WriteJson(payload, WorkbenchJsonContext.Default.ValidateOutput);
        }
        else
        {
            if (verbose)
            {
                Console.WriteLine($"Work items scanned: {result.WorkItemCount}");
                Console.WriteLine($"Markdown files scanned: {result.MarkdownFileCount}");
            }
            if (result.Errors.Count > 0)
            {
                Console.WriteLine("Errors:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error}");
                }
            }
            if (result.Warnings.Count > 0)
            {
                Console.WriteLine("Warnings:");
                foreach (var warning in result.Warnings)
                {
                    Console.WriteLine($"- {warning}");
                }
            }
            if (result.Errors.Count == 0 && result.Warnings.Count == 0)
            {
                Console.WriteLine("Validation passed.");
            }
        }
        SetExitCode(exit);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        SetExitCode(2);
    }
}, repoOption, formatOption, strictOption, verboseOption);
root.AddCommand(validateCommand);

var exitCode = await root.InvokeAsync(args);
return Environment.ExitCode != 0 ? Environment.ExitCode : exitCode;
