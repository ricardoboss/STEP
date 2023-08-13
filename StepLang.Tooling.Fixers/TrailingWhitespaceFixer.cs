namespace StepLang.Formatters;

public class TrailingWhitespaceFixer : IStringFixer
{
    public string Name => "TrailingWhitespaceFixer";

    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
    {
        var fixedInput = input.WithoutTrailingWhitespace();

        return Task.FromResult<StringFixResult>(new(true, null, fixedInput));
    }
}