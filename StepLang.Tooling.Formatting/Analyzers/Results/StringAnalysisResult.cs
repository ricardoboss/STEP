namespace StepLang.Tooling.Formatting.Analyzers.Results;

public record StringAnalysisResult : AnalysisResult
{
	public static StringAnalysisResult FromInputAndFix(string input, string fixedString)
	{
		return new StringAnalysisResult(!string.Equals(input, fixedString, StringComparison.Ordinal), fixedString);
	}

	private StringAnalysisResult(bool FixRequired, string? FixedString) : base(FixRequired)
	{
		this.FixedString = FixedString;
	}

	public string? FixedString { get; }
}
