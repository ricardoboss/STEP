using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class LineEndingFixer : IStringFixer
{
    public string Name => "LineEndingFixer";

    public Task<StringFixResult> FixAsync(string input, FixerConfiguration configuration, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fixedString = string.Join(configuration.LineEndings, input.SplitLines());

        return Task.FromResult(StringFixResult.FromInputAndFix(input, fixedString));
    }
}