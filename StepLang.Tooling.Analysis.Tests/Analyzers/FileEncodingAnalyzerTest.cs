using StepLang.Tooling.Analysis.Analyzers;
using System.Text;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public static class FileEncodingAnalyzerTest
{
	[Fact]
	public static void TestLeavesReaderOpenAfterGettingEncoding()
	{
		using var stream = new MemoryStream();

		_ = FileEncodingAnalyzer.GetEncoding(stream, FileEncodingAnalyzer.DefaultEncoding);

		Assert.True(stream.CanRead);
	}

	[Fact]
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

			Assert.NotNull(result.FixedFile);

			var fixedFileContents = await File.ReadAllTextAsync(result.FixedFile.FullName, CancellationToken.None);

			Assert.Equal(contents, fixedFileContents);
		}
		finally
		{
			File.Delete(targetFileName);
		}
	}
}
