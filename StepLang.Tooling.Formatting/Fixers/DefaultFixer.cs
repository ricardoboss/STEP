using System.Diagnostics;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

/// <summary>
/// The default fixer. This fixer applies fixes to actual files.
/// </summary>
public class DefaultFixer : BaseFixer
{
    /// <inheritdoc />
    protected override async Task<FixerResult> ApplyResult(AnalysisResult result, FileInfo file,
        CancellationToken cancellationToken)
    {
        Stopwatch sw = new();

        sw.Start();

        switch (result)
        {
            case StringAnalysisResult stringFixResult:
                await File.WriteAllTextAsync(file.FullName, stringFixResult.FixedString, cancellationToken);
                break;
            case FileAnalysisResult fileFixResult:
                fileFixResult.FixedFile.MoveTo(file.FullName, true);
                break;
            default:
                throw new NotSupportedException($"Unknown fix result type '{result.GetType().FullName}'");
        }

        sw.Stop();

        return FixerResult.Applied(1, sw.Elapsed);
    }
}