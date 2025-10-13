using StepLang.Tooling.Analysis.Analyzers;
using System.Text;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public static class FileEncodingAnalyzerTest
{
	[Test]
	public static void TestLeavesReaderOpenAfterGettingEncoding()
	{
		using var stream = new MemoryStream();

		_ = FileEncodingAnalyzer.GetEncoding(stream, FileEncodingAnalyzer.DefaultEncoding);

		Assert.That(stream.CanRead, Is.True);
	}

	[Test]
	public static async Task TestKeepsFileContents()
	{
		const string contents = "Hello!";

		var targetFileName = Path.GetTempFileName();

		try
		{
			await using (var writer = new StreamWriter(File.OpenWrite(targetFileName), Encoding.UTF32))
				await writer.WriteAsync(contents);

			var fixer = new FileEncodingAnalyzer();

			var result = await fixer.AnalyzeAsync(new FileInfo(targetFileName), CancellationToken.None);

			Assert.That(result.FixedFile, Is.Not.Null);

			var fixedFileContents = await File.ReadAllTextAsync(result.FixedFile.FullName, CancellationToken.None);

			Assert.That(fixedFileContents, Is.EqualTo(contents));
		}
		finally
		{
			File.Delete(targetFileName);
		}
	}
}
