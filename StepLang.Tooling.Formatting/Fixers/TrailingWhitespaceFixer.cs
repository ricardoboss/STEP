using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class TrailingWhitespaceFixer : IStringFixer
{
    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fixedString = input.WithoutTrailingWhitespace();

        return Task.FromResult(StringFixResult.FromInputAndFix(input, fixedString));
    }
}