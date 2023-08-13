using System.Diagnostics;

namespace StepLang.Formatters;

public record AfterFixerRanEventArgs(FileInfo File, IFixer Fixer, FixResult Result);

public record BeforeFixerRanEventArgs(FileInfo File, IFixer Fixer);

public class DefaultFixApplicator : BaseFixApplicator
{
    public event EventHandler<BeforeFixerRanEventArgs>? BeforeFixerRun;

    public event EventHandler<AfterFixerRanEventArgs>? BeforeApplyFix;

    public event EventHandler<AfterFixerRanEventArgs>? AfterApplyFix;

    private void OnBeforeFixerRun(FileInfo file, IFixer fixer) => BeforeFixerRun?.Invoke(this, new(file, fixer));

    private void OnBeforeApplyFix(FileInfo file, IFixer fixer, FixResult result) => BeforeApplyFix?.Invoke(this, new(file, fixer, result));

    private void OnAfterApplyFix(FileInfo file, IFixer fixer, FixResult result) => AfterApplyFix?.Invoke(this, new(file, fixer, result));

    public override bool ThrowOnFailure { get; init; } = true;

    public override bool DryRun { get; init; }

    public override async Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, FileInfo file,
        CancellationToken cancellationToken = default)
    {
        OnBeforeFixerRun(file, fixer);

        Stopwatch sw = new();

        sw.Start();

        FixResult result;
        try
        {
            result = fixer switch
            {
                IStringFixer stringFixer => await RunStringFixer(stringFixer, file, cancellationToken),
                IFileFixer fileFixer => await RunFileFixer(fileFixer, file, cancellationToken),
                _ => throw new NotImplementedException($"Unknown fixer type '{fixer.GetType().FullName}'"),
            };

            sw.Stop();
        }
        catch (Exception e)
        {
            sw.Stop();

            if (ThrowOnFailure)
                throw new FixerException(fixer, $"Failed to run fixer '{fixer.Name}' on file '{file.FullName}'", e);

            return new(0, 1, sw.Elapsed);
        }

        if (!result.FixRequired)
            return new(0, 0, sw.Elapsed);

        return await ApplyFixer(fixer, file, result, sw.Elapsed, cancellationToken);
    }

    private async Task<FixApplicatorResult> ApplyFixer(IFixer fixer, FileInfo file, FixResult result, TimeSpan fixDuration, CancellationToken cancellationToken)
    {
        OnBeforeApplyFix(file, fixer, result);

        if (!DryRun)
        {
            switch (result)
            {
                case StringFixResult stringFixResult:
                    await File.WriteAllTextAsync(file.FullName, stringFixResult.FixedString, cancellationToken);
                    break;
                case FileFixResult fileFixResult:
                    fileFixResult.FixedFile.MoveTo(file.FullName, true);
                    break;
                default:
                    throw new NotImplementedException($"Unknown fix result type '{result.GetType().FullName}'");
            }
        }

        OnAfterApplyFix(file, fixer, result);

        return new(1, 0, fixDuration);
    }

    private static async Task<FileFixResult> RunFileFixer(IFileFixer fixer, FileInfo file, CancellationToken cancellationToken)
    {
        return await fixer.FixAsync(file, cancellationToken);
    }

    private static async Task<StringFixResult> RunStringFixer(IStringFixer fixer, FileSystemInfo file, CancellationToken cancellationToken)
    {
        var contents = await File.ReadAllTextAsync(file.FullName, cancellationToken);

        return await fixer.FixAsync(contents, cancellationToken);
    }
}