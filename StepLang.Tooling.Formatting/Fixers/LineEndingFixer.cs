using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class LineEndingFixer : IStringFixer
{
    public const string DefaultLineEnding = "\n";

    public string Name => "LineEndingFixer";

    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
    {
        var fixedString = input.Replace("\r\n", DefaultLineEnding);

        return Task.FromResult(StringFixResult.FromInputAndFix(input, fixedString));
    }
}