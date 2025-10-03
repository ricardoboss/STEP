using StepLang.Tooling.Formatting.Analyzers.Results;
using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Analyzers;

public interface IFileAnalyzer : IAnalyzer
{
	/// <summary>
	/// Applies a fix to the given file. The fix should NOT override the file, but instead return a new file.
	/// </summary>
	/// <param name="original">The file to fix.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A result with a fixed file.</returns>
	Task<FileAnalysisResult> AnalyzeAsync(FileInfo original, CancellationToken cancellationToken = default);

	/// <inheritdoc />
	async Task<IAnalysisResult> IAnalyzer.AnalyzeAsync(IFixerSource source, CancellationToken cancellationToken)
	{
		if (source.File is not { } file)
			throw new ArgumentException("No file was specified in the source", nameof(source));

		return await AnalyzeAsync(file, cancellationToken);
	}
}
