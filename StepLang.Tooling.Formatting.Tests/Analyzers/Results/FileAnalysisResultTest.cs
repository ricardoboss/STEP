using StepLang.Tooling.Formatting.Analyzers.Results;
using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Tests.Analyzers.Results;

public static class FileAnalysisResultTest
{
	[Fact]
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
			Assert.Equal("Fixed", targetFileContents.TrimEnd());

			fixerSourceMock.VerifyAll();
		}
		finally
		{
			File.Delete(fixedFileName);
			File.Delete(targetFileName);
		}
	}

	[Fact]
	public static async Task TestThrowsWhenApplyingFixWithRetainOriginalResult()
	{
		var fixerSourceMock = new Mock<IFixerSource>();

		var result = FileAnalysisResult.RetainOriginal();

		var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
			await result.ApplyFixAsync(fixerSourceMock.Object, CancellationToken.None));

		Assert.Equal("No fix is available for this result", ex.Message);

		fixerSourceMock.VerifyAll();
	}
}
