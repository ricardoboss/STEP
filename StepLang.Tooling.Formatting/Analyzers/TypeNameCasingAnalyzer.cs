using StepLang.Tokenizing;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

public class TypeNameCasingAnalyzer : IStringAnalyzer
{
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