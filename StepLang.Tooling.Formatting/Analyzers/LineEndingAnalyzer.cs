using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// Replaces all line endings with the default line ending (<c>\n</c>).
/// </summary>
public class LineEndingAnalyzer : IStringAnalyzer
{
    private const string DefaultLineEnding = "\n";

    /// <inheritdoc />
    public Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fixedString = string.Join(DefaultLineEnding, input.SplitLines());

        return Task.FromResult(StringAnalysisResult.FromInputAndFix(input, fixedString));
    }
}