using StepLang.Tooling.Formatting.Analyzers.Results;
using System.Text;

namespace StepLang.Tooling.Formatting.Analyzers;

public class FileEncodingAnalyzer : IFileAnalyzer
{
	private static readonly Encoding DefaultEncoding = Encoding.UTF8;

	public async Task<FileAnalysisResult> AnalyzeAsync(FileInfo original, CancellationToken cancellationToken = default)
	{
		await using var stream = new FileStream(original.FullName, FileMode.Open, FileAccess.Read);

		var usedEncoding = GetEncoding(stream, DefaultEncoding);
		if (usedEncoding.Equals(DefaultEncoding))
			return FileAnalysisResult.RetainOriginal();

		var tempFile = Path.GetTempFileName();

		await using var tempWriter = new StreamWriter(tempFile, false, DefaultEncoding);
		using var fileReader = new StreamReader(stream, usedEncoding);
		await tempWriter.WriteAsync(await fileReader.ReadToEndAsync(cancellationToken));
		await tempWriter.FlushAsync(cancellationToken);

		return FileAnalysisResult.FixedAt(AnalysisSeverity.Warning, new FileInfo(tempFile));
	}

	private static Encoding GetEncoding(Stream stream, Encoding fallback)
	{
		using var reader = new StreamReader(stream, fallback, true);

		// Detect byte order mark if any - otherwise assume default
		_ = reader.Peek();

		return reader.CurrentEncoding;
	}
}
