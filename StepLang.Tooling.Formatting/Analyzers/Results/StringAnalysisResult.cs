using StepLang.Tooling.Formatting.Analyzers.Source;
using System.Text;

namespace StepLang.Tooling.Formatting.Analyzers.Results;

public record StringAnalysisResult : IApplicableAnalysisResult
{
	public static StringAnalysisResult FromInputAndFix(string input, string fixedString)
	{
		var inputChanged = !string.Equals(input, fixedString, StringComparison.Ordinal);

		return new StringAnalysisResult(inputChanged, inputChanged ? fixedString : null);
	}

	private StringAnalysisResult(bool shouldFix, string? fixedString)
	{
		FixedString = fixedString;
		ShouldFix = shouldFix;
	}

	public string? FixedString { get; }

	public bool ShouldFix { get; }

	public bool FixAvailable => ShouldFix;

	public async Task ApplyFixAsync(IFixerSource source, CancellationToken cancellationToken = default)
	{
		if (FixedString is null)
			throw new InvalidOperationException("No fix is available for this result");

		if (source is not { File: { } targetFile })
			throw new ArgumentException("The source does not point to an actual file; no way to apply fixes",
				nameof(source));

		Encoding originalEncoding;
		await using (var stream = targetFile.OpenRead())
			originalEncoding = FileEncodingAnalyzer.GetEncoding(stream, FileEncodingAnalyzer.DefaultEncoding);

		await File.WriteAllTextAsync(targetFile.FullName, FixedString, originalEncoding, cancellationToken);
	}
}
