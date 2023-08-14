using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class LineEndingFixer : IStringFixer
{
    public string Name => "LineEndingFixer";

    public Task<StringFixResult> FixAsync(string input, FixerConfiguration configuration, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var lineEnding = configuration.LineEnding switch
        {
            FixerConfiguration.LineEndings.Lf => "\n",
            FixerConfiguration.LineEndings.CrLf => "\r\n",
            _ => throw new ArgumentOutOfRangeException(nameof(configuration.LineEnding), configuration.LineEnding, "Unknown line ending type."),
        };

        var fixedString = string.Join(lineEnding, input.SplitLines());

        return Task.FromResult(StringFixResult.FromInputAndFix(input, fixedString));
    }
}