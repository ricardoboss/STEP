using StepLang.Tooling.Formatting.Analyzers.Source;
using System.Text;

namespace StepLang.Tooling.Formatting.Analyzers.Results;

public record StringAnalysisResult : IApplicableAnalysisResult
{
	public static StringAnalysisResult FromInputAndFix(AnalysisSeverity severity, string input, string fixedString)
	{
		return new StringAnalysisResult(severity, !string.Equals(input, fixedString, StringComparison.Ordinal), fixedString);
	}

	private StringAnalysisResult(AnalysisSeverity severity, bool fixRequired, string? fixedString)
	{
		Severity = severity;
		FixedString = fixedString;
		ShouldFix = fixRequired;
	}

	public string? FixedString { get; }

	public AnalysisSeverity Severity { get; }

	public bool ShouldFix { get; }

	public bool FixAvailable => FixedString != null;

	public async Task ApplyFixAsync(IFixerSource source, CancellationToken cancellationToken = default)
	{
		if (FixedString is null)
			throw new InvalidOperationException("No fix is available for this result");

		if (source is not { File: { } targetFile })
			throw new ArgumentException("The source does not point to an actual file; no way to apply fixes",
				nameof(source));

		await File.WriteAllTextAsync(targetFile.FullName, FixedString, Encoding.UTF8, cancellationToken);
	}
}
