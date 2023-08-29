namespace StepLang.Tooling.Formatting.Analyzers.Results;

public record StringAnalysisResult(bool FixRequired, string? FixedString) : AnalysisResult(FixRequired)
{
    public static StringAnalysisResult FromInputAndFix(string input, string fixedString) => new(!string.Equals(input, fixedString, StringComparison.Ordinal), fixedString);
}