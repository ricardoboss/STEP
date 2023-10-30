using StepLang.Tokenizing;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// Checks the casing of language keywords and converts them to the canonical casing (i.e. lowercase).
/// </summary>
/// <seealso cref="TokenTypes.ToCode"/>
public class KeywordCasingAnalyzer : IStringAnalyzer
{
    /// <inheritdoc />
    public Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default)
    {
        var fixedString = input.SelectWords(word =>
        {
            if (word.TryParseKeyword(out var keywordToken))
                return keywordToken.ToCode();

            return word;
        });

        return Task.FromResult(StringAnalysisResult.FromInputAndFix(input, fixedString));
    }
}