using StepLang.Tooling.Formatting.Analyzers;
using StepLang.Tooling.Formatting.Analyzers.Results;
using StepLang.Tooling.Formatting.Analyzers.Source;
using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.Tests.Fixers;

public static class DryRunFixerTest
{
	[Fact]
	public static async Task TestCountsApplicableFixes()
	{
		var analyzerMock = new Mock<IAnalyzer>();
		var sourceMock = new Mock<IFixerSource>();
		var analysisResultMock = new Mock<IApplicableAnalysisResult>();

		analyzerMock
			.Setup(a => a.AnalyzeAsync(sourceMock.Object, It.IsAny<CancellationToken>()))
			.ReturnsAsync(analysisResultMock.Object)
			.Verifiable();

		analysisResultMock
			.SetupGet(r => r.ShouldFix)
			.Returns(true)
			.Verifiable();

		analysisResultMock
			.SetupGet(r => r.FixAvailable)
			.Returns(true)
			.Verifiable();

		var fixer = new DryRunFixer();

		var result = await fixer.FixAsync(analyzerMock.Object, sourceMock.Object, CancellationToken.None);

		Assert.Equal(1, result.AppliedFixes);

		analyzerMock.VerifyAll();
		sourceMock.VerifyAll();
	}
}
