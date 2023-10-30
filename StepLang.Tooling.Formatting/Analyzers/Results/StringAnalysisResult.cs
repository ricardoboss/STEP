namespace StepLang.Tooling.Formatting.Analyzers.Results;

/// <summary>
/// Represents the result of analyzing source code in form of a string.
/// </summary>
public record StringAnalysisResult : AnalysisResult
{
    /// <summary>
    /// Creates a new instance of <see cref="StringAnalysisResult"/> with the fix provided in <paramref name="fixedString"/>.
    /// </summary>
    /// <param name="input">The original string.</param>
    /// <param name="fixedString">The string with the fix.</param>
    /// <returns>A new instance of <see cref="StringAnalysisResult"/> with the fix provided in <paramref name="fixedString"/>.</returns>
    public static StringAnalysisResult FromInputAndFix(string input, string fixedString) => new(!string.Equals(input, fixedString, StringComparison.Ordinal), fixedString);

    private StringAnalysisResult(bool FixRequired, string? FixedString) : base(FixRequired)
    {
        this.FixedString = FixedString;
    }

    /// <summary>
    /// The string with any fixes applied.
    /// </summary>
    public string? FixedString { get; }
}