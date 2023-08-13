using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class TrailingWhitespaceFixer : IStringFixer
{
    public string Name => "TrailingWhitespaceFixer";

    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
    {
        var fixedString = input.WithoutTrailingWhitespace();

        return Task.FromResult(StringFixResult.FromInputAndFix(input, fixedString));
    }
}