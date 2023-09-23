using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.Tooling.Formatting.AnalyzerSet;
using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class FormatCommand : AsyncCommand<FormatCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
        [CommandArgument(0, "<file-or-dir>")]
        [Description("One or more paths to files or directories to format.")]
        public string[]? FilesOrDirs { get; init; }

        [CommandOption("-d|--dry-run")]
        [DefaultValue(false)]
        [Description("Run the formatter without modifying any files.")]
        public bool DryRun { get; init; }

        [CommandOption("-e|--set-exit-code")]
        [DefaultValue(false)]
        [Description("Set the exit code to be non-zero if any files were/would have been modified.")]
        public bool SetExitCode { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.DryRun)
            AnsiConsole.MarkupLineInterpolated($"[yellow][bold]Dry run mode enabled.[/] No files will be modified.[/]");

        var filesOrDirs = settings.FilesOrDirs;
        if (filesOrDirs is null or { Length: 0 })
            filesOrDirs = new[] { "." };

        IFixer fixer = settings.DryRun ? new DryRunFixer() : new DefaultFixer();

        var files = new HashSet<FileInfo>();

        fixer.AfterApplyFix += (_, f) =>
        {
            files.Add(f.File);

            AnsiConsole.MarkupLineInterpolated(
                $"Applied analyzer [darkmagenta]'{f.Analyzer.Name}'[/] to [darkcyan]'{f.File.Name}'[/]");
        };

        var analyzers = new DefaultAnalyzerSet();

        var results = await filesOrDirs
            .ToAsyncEnumerable()
            .AggregateAwaitAsync(
                FixerResult.None,
                async (current, fileOrDir) => current + await Fix(fileOrDir, fixer, analyzers)
            );

        AnsiConsole.MarkupLineInterpolated(
            $"{(settings.DryRun ? "Would have fixed" : "Fixed")} [green]{files.Count.ToString(CultureInfo.InvariantCulture)}[/] file(s) in [darkcyan]{results.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture)}[/] seconds.");

        if (settings.SetExitCode)
            return results.AppliedFixes > 0 ? 1 : 0;

        return 0;
    }

    private static async Task<FixerResult> Fix(string fileOrDir, IFixer fixer,
        IAnalyzerSet analyzers)
    {
        FixerResult result;
        if (File.Exists(fileOrDir))
        {
            var file = new FileInfo(fileOrDir);
            AnsiConsole.MarkupLineInterpolated($"Formatting file [darkblue]'{file.FullName}'[/]...");
            result = await fixer.FixAsync(analyzers, file);
        }
        else if (Directory.Exists(fileOrDir))
        {
            var dir = new DirectoryInfo(fileOrDir);
            AnsiConsole.MarkupLineInterpolated($"Formatting directory [darkblue]'{dir.FullName}'[/]...");
            result = await fixer.FixAsync(analyzers, dir);
        }
        else
        {
            AnsiConsole.MarkupLineInterpolated($"The path [darkblue]'{fileOrDir}'[/] is not a file or directory.");

            return FixerResult.None;
        }

        return result;
    }
}