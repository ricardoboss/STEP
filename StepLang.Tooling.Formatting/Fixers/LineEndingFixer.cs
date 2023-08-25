using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class LineEndingFixer : IStringFixer
{
    private const string DefaultLineEnding = "\n";

    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fixedString = string.Join(DefaultLineEnding, input.SplitLines());

        return Task.FromResult(StringFixResult.FromInputAndFix(input, fixedString));
    }
}