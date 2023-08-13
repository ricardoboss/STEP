namespace StepLang.Formatters;

public class LineEndingFixer : IStringFixer
{
    public const string DefaultLineEnding = "\n";

    public string Name => "LineEndingFixer";

    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
    {
        var fixedString = input.Replace("\r\n", DefaultLineEnding);

        return Task.FromResult<StringFixResult>(new(true, null, fixedString));
    }
}