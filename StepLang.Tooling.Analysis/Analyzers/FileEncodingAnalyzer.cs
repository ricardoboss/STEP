using StepLang.Tooling.Analysis.Analyzers.Results;
using System.Text;

namespace StepLang.Tooling.Analysis.Analyzers;

public class FileEncodingAnalyzer : IFileAnalyzer
{
	public static readonly Encoding DefaultEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

	public async Task<FileAnalysisResult> AnalyzeAsync(FileInfo original, CancellationToken cancellationToken = default)
	{
		await using var originalStream = new FileStream(original.FullName, FileMode.Open, FileAccess.Read);

		var usedEncoding = GetEncoding(originalStream, DefaultEncoding);
		if (usedEncoding.Equals(DefaultEncoding))
			return FileAnalysisResult.RetainOriginal();

		var tempFile = Path.GetTempFileName();

		await using var tempWriter = new StreamWriter(tempFile, false, DefaultEncoding);
		using var fileReader = new StreamReader(originalStream, usedEncoding);
		var originalContents = await fileReader.ReadToEndAsync(cancellationToken);
		await tempWriter.WriteAsync(originalContents);
		await tempWriter.FlushAsync(cancellationToken);

		return FileAnalysisResult.FixedAt(new FileInfo(tempFile));
	}

	public static Encoding GetEncoding(Stream stream, Encoding fallback)
	{
		if (!stream.CanSeek)
			throw new ArgumentException(
				"Detecting file encoding requires seeking through the stream; the given stream does not support seeking",
				nameof(stream)
			);

		Encoding actualEncoding;
		using (var reader = new StreamReader(stream, fallback, detectEncodingFromByteOrderMarks: true, leaveOpen: true))
		{
			// Detect byte order mark if any - otherwise assume default
			_ = reader.Peek();

			actualEncoding = reader.CurrentEncoding;
		}

		stream.Position = 0;

		return actualEncoding;
	}

	public string Name => "File Encoding";
}
