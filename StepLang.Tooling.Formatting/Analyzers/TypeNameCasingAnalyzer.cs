using StepLang.Tokenizing;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// Checks the casing of built-in type names and converts them to lower case.
/// </summary>
/// <seealso cref="TokenTypes.IsKnownTypeName"/>
public class TypeNameCasingAnalyzer : IStringAnalyzer
{
    /// <inheritdoc/>
    public Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default)
    {
        var fixedString = input.SelectWords(word =>
        {
            if (word.IsKnownTypeName())
                return word.ToLowerInvariant();

            return word;
        });

        return Task.FromResult(StringAnalysisResult.FromInputAndFix(input, fixedString));
    }
}