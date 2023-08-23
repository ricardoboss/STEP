using StepLang.Tokenizing;
using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class KeywordCasingFixer : IStringFixer
{
    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
    {
        var fixedString = input.SelectWords(word =>
        {
            if (word.TryParseKeyword(out var keywordToken))
                return keywordToken.ToCode();

            return word;
        });

        return Task.FromResult(StringFixResult.FromInputAndFix(input, fixedString));
    }
}