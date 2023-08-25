using StepLang.Tokenizing;
using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class TypeNameCasingFixer : IStringFixer
{
    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
    {
        var fixedString = input.SelectWords(word =>
        {
            if (word.IsKnownTypeName())
                return word.ToLowerInvariant();

            return word;
        });

        return Task.FromResult(StringFixResult.FromInputAndFix(input, fixedString));
    }
}