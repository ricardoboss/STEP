namespace StepLang.Tooling.Formatting.Fixers.Results;

public record StringFixResult(bool FixRequired, string? FixedString) : FixResult(FixRequired)
{
    public static StringFixResult FromInputAndFix(string input, string fixedString) => new(!string.Equals(input, fixedString, StringComparison.Ordinal), fixedString);
}