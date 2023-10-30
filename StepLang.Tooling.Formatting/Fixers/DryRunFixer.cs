using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

/// <summary>
/// A fixer that does not change any files and can be used for testing.
/// </summary>
public class DryRunFixer : BaseFixer
{
    /// <inheritdoc />
    protected override Task<FixerResult> ApplyResult(AnalysisResult result, FileInfo file,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(FixerResult.Applied(1, TimeSpan.Zero));
    }
}