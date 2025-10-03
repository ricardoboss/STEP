using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Analyzers.Results;

public record FileAnalysisResult : IApplicableAnalysisResult
{
	public static FileAnalysisResult FixedAt(FileInfo fixedFile)
	{
		return new FileAnalysisResult(true, fixedFile);
	}

	public static FileAnalysisResult RetainOriginal()
	{
		return new FileAnalysisResult(false, null);
	}

	private FileAnalysisResult(bool shouldFix, FileInfo? fixedFile)
	{
		FixedFile = fixedFile;
		ShouldFix = shouldFix;
	}

	public FileInfo? FixedFile { get; }

	public bool ShouldFix { get; }

	public bool FixAvailable => FixedFile != null;

	public Task ApplyFixAsync(IFixerSource source, CancellationToken cancellationToken = default)
	{
		if (FixedFile is null)
			throw new InvalidOperationException("No fix is available for this result");

		if (source is not { File: { } targetFile })
			throw new ArgumentException("The source does not point to an actual file; no way to apply fixes",
				nameof(source));

		FixedFile.MoveTo(targetFile.FullName, overwrite: true);

		return Task.CompletedTask;
	}
}
