using StepLang.Tokenizing;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

public class KeywordCasingAnalyzer : IStringAnalyzer
{
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