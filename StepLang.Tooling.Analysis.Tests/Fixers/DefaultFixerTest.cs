using StepLang.Tooling.Analysis.Analyzers;
using StepLang.Tooling.Analysis.Analyzers.Results;
using StepLang.Tooling.Analysis.Analyzers.Source;
using StepLang.Tooling.Analysis.Fixers;

namespace StepLang.Tooling.Analysis.Tests.Fixers;

public static class DefaultFixerTest
{
	[Fact]
	public static async Task TestAppliesFixes()
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

		var fixer = new DefaultFixer();

		var result = await fixer.FixAsync(analyzerMock.Object, sourceMock.Object, CancellationToken.None);

		Assert.Equal(1, result.AppliedFixes);

		analyzerMock.VerifyAll();
		sourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}
}
