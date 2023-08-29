using System.Globalization;
using Pastel;
using StepLang.Tooling.Formatting.AnalyzerSet;
using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.CLI;

internal static class FormatCommand
{
    private static VerbosityWriter stdout = new(Console.Out, Verbosity.Normal);
    private static VerbosityWriter stderr = new(Console.Error, Verbosity.Normal);

    public static async Task<int> Invoke(string[] filesOrDirs, bool setExitCode, bool dryRun, Verbosity verbosity)
    {
        stdout = new(Console.Out, verbosity);
        stderr = new(Console.Error, verbosity);

        if (dryRun)
            await stdout.Normal("Dry run mode enabled. No files will be modified.".Pastel(ConsoleColor.DarkYellow));

        if (filesOrDirs.Length == 0)
            filesOrDirs = new[] { "." };

        IFixer fixer = dryRun ? new DryRunFixer() : new DefaultFixer();

        var files = new HashSet<FileInfo>();

        fixer.AfterApplyFix += async (_, f) =>
        {
            files.Add(f.File);

            await stdout.Verbose(
                $"Applied analyzer '{f.Analyzer.Name.Pastel(ConsoleColor.DarkMagenta)}' to '{f.File.Name.Pastel(ConsoleColor.DarkCyan)}'");
        };

        var analyzers = new DefaultAnalyzerSet();

        var results = await filesOrDirs
            .ToAsyncEnumerable()
            .AggregateAwaitAsync(
                FixerResult.None,
                async (current, fileOrDir) => current + await Fix(fileOrDir, fixer, analyzers)
            );

        await stdout.Normal(
            $"{(dryRun ? "Would have fixed" : "Fixed")} {files.Count.ToString(CultureInfo.InvariantCulture).Pastel(ConsoleColor.Green)} file(s) in {results.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture).Pastel(ConsoleColor.DarkCyan)} seconds.");

        if (setExitCode)
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
            await stdout.Normal($"Formatting file '{file.FullName.Pastel(ConsoleColor.DarkBlue)}'...");
            result = await fixer.FixAsync(analyzers, file);
        }
        else if (Directory.Exists(fileOrDir))
        {
            var dir = new DirectoryInfo(fileOrDir);
            await stdout.Normal($"Formatting directory '{dir.FullName.Pastel(ConsoleColor.DarkBlue)}'...");
            result = await fixer.FixAsync(analyzers, dir);
        }
        else
        {
            await stderr.Loud($"The path '{fileOrDir.Pastel(ConsoleColor.DarkBlue)}' is not a file or directory.");

            return FixerResult.None;
        }

        return result;
    }
}