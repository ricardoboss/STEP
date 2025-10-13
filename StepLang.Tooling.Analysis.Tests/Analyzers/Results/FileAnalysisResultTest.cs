using StepLang.Tooling.Analysis.Analyzers.Results;
using StepLang.Tooling.Analysis.Analyzers.Source;

namespace StepLang.Tooling.Analysis.Tests.Analyzers.Results;

public static class FileAnalysisResultTest
{
	[Test]
	public static async Task TestOverwritesOldFileOnFixApplication()
	{
		var fixedFileName = Path.GetTempFileName();
		var targetFileName = Path.GetTempFileName();

		try
		{
			await using (var writer = File.CreateText(targetFileName))
				await writer.WriteLineAsync("Original");

			await using (var writer = File.CreateText(fixedFileName))
				await writer.WriteLineAsync("Fixed");

			var fixerSourceMock = new Mock<IFixerSource>();

			fixerSourceMock
				.SetupGet(s => s.File)
				.Returns(new FileInfo(targetFileName))
				.Verifiable();

			var result = FileAnalysisResult.FixedAt(new FileInfo(fixedFileName));

			await result.ApplyFixAsync(fixerSourceMock.Object, CancellationToken.None);

			var targetFileContents = await File.ReadAllTextAsync(targetFileName, CancellationToken.None);
			Assert.That(targetFileContents.TrimEnd(), Is.EqualTo("Fixed"));

			fixerSourceMock.VerifyAll();
		}
		finally
		{
			File.Delete(fixedFileName);
			File.Delete(targetFileName);
		}
	}

	[Test]
	public static void TestThrowsWhenApplyingFixWithRetainOriginalResult()
	{
		var fixerSourceMock = new Mock<IFixerSource>();

		var result = FileAnalysisResult.RetainOriginal();

		var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
				result.ApplyFixAsync(fixerSourceMock.Object, CancellationToken.None));

		Assert.That(ex.Message, Is.EqualTo("No fix is available for this result"));

		fixerSourceMock.VerifyAll();
	}
}
