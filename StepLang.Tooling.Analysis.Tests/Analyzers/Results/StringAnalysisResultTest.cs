using StepLang.Tooling.Analysis.Analyzers;
using StepLang.Tooling.Analysis.Analyzers.Results;
using StepLang.Tooling.Analysis.Analyzers.Source;
using System.Text;

namespace StepLang.Tooling.Analysis.Tests.Analyzers.Results;

public static class StringAnalysisResultTest
{
	[Test]
	public static async Task TestOverwritesSourceFileOnFixApplication()
	{
		var targetFileName = Path.GetTempFileName();

		try
		{
			await using (var writer = File.CreateText(targetFileName))
				await writer.WriteLineAsync("Original");

			var fixerSourceMock = new Mock<IFixerSource>();

			fixerSourceMock
				.SetupGet(s => s.File)
				.Returns(new FileInfo(targetFileName))
				.Verifiable();

			var result = StringAnalysisResult.FromInputAndFix("Original", "Fixed");

			await result.ApplyFixAsync(fixerSourceMock.Object, CancellationToken.None);

			var targetFileContents = await File.ReadAllTextAsync(targetFileName, CancellationToken.None);
			Assert.That(targetFileContents.TrimEnd(), Is.EqualTo("Fixed"));

			fixerSourceMock.VerifyAll();
		}
		finally
		{
			File.Delete(targetFileName);
		}
	}

	[Test]
	public static async Task TestKeepsOriginalEncoding()
	{
		var targetFileName = Path.GetTempFileName();

		try
		{
			var intendedEncoding = Encoding.UTF32;
			var fallbackEncoding = Encoding.UTF8;

			await using (var writer = new StreamWriter(File.OpenWrite(targetFileName), intendedEncoding))
				await writer.WriteLineAsync("Original");

			var fixerSourceMock = new Mock<IFixerSource>();

			fixerSourceMock
				.SetupGet(s => s.File)
				.Returns(new FileInfo(targetFileName))
				.Verifiable();

			var result = StringAnalysisResult.FromInputAndFix("Original", "Fixed");

			await result.ApplyFixAsync(fixerSourceMock.Object, CancellationToken.None);

			await using (var stream = new FileStream(targetFileName, FileMode.Open, FileAccess.Read))
			{
				// explicitly specify a different default encoding
				var actualEncoding = FileEncodingAnalyzer.GetEncoding(stream, fallbackEncoding);

				Assert.That(actualEncoding, Is.EqualTo(intendedEncoding));
			}

			fixerSourceMock.VerifyAll();
		}
		finally
		{
			File.Delete(targetFileName);
		}
	}

	[Test]
	public static void TestThrowsWhenApplyingFixWithRetainOriginalResult()
	{
		var fixerSourceMock = new Mock<IFixerSource>();

		var result = StringAnalysisResult.FromInputAndFix("Original", "Original");

		var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
				result.ApplyFixAsync(fixerSourceMock.Object, CancellationToken.None));

		Assert.That(ex.Message, Is.EqualTo("No fix is available for this result"));

		fixerSourceMock.VerifyAll();
	}
}
