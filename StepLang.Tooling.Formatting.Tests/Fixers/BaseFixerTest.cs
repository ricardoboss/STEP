using StepLang.Tooling.Formatting.Analyzers;
using StepLang.Tooling.Formatting.Analyzers.Results;
using StepLang.Tooling.Formatting.Analyzers.Source;
using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.Tests.Fixers;

public static class BaseFixerTest
{
	[Fact]
	public static async Task TestInvokesOnUnfixable()
	{
		var analyzerMock = new Mock<IAnalyzer>();
		var fixerSourceMock = new Mock<IFixerSource>();
		var analysisResultMock = new Mock<IApplicableAnalysisResult>();

		analyzerMock
			.Setup(a => a.AnalyzeAsync(fixerSourceMock.Object, It.IsAny<CancellationToken>()))
			.ReturnsAsync(analysisResultMock.Object)
			.Verifiable();

		analysisResultMock
			.SetupGet(r => r.ShouldFix)
			.Returns(true)
			.Verifiable();

		analysisResultMock
			.SetupGet(r => r.FixAvailable)
			.Returns(false)
			.Verifiable();

		var fixer = new TestBaseFixer();

		var unfixableInvoked = false;
		fixer.OnUnfixable += (_, _) => unfixableInvoked = true;

		var result = await fixer.FixAsync(analyzerMock.Object, fixerSourceMock.Object, CancellationToken.None);

		Assert.Equal(0, result.AppliedFixes);
		Assert.True(unfixableInvoked);

		analyzerMock.VerifyAll();
		fixerSourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}

	[Fact]
	public static async Task TestBailsIfShouldntFix()
	{
		var analyzerMock = new Mock<IAnalyzer>();
		var fixerSourceMock = new Mock<IFixerSource>();
		var analysisResultMock = new Mock<IApplicableAnalysisResult>();

		analyzerMock
			.Setup(a => a.AnalyzeAsync(fixerSourceMock.Object, It.IsAny<CancellationToken>()))
			.ReturnsAsync(analysisResultMock.Object)
			.Verifiable();

		analysisResultMock
			.SetupGet(r => r.ShouldFix)
			.Returns(false)
			.Verifiable();

		var fixer = new TestBaseFixer();

		var result = await fixer.FixAsync(analyzerMock.Object, fixerSourceMock.Object, CancellationToken.None);

		Assert.Equal(0, result.AppliedFixes);

		analyzerMock.VerifyAll();
		fixerSourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}

	[Fact]
	public static async Task TestDoesntThrowIfThrowOnFailureIsFalse()
	{
		var analyzerMock = new Mock<IAnalyzer>();
		var fixerSourceMock = new Mock<IFixerSource>();
		var analysisResultMock = new Mock<IApplicableAnalysisResult>();

		analyzerMock
			.Setup(a => a.AnalyzeAsync(fixerSourceMock.Object, It.IsAny<CancellationToken>()))
			.ThrowsAsync(new TestAnalysisException())
			.Verifiable();

		var fixer = new TestBaseFixer
		{
			ThrowOnFailure = false,
		};

		var result = await fixer.FixAsync(analyzerMock.Object, fixerSourceMock.Object, CancellationToken.None);

		Assert.Equal(0, result.AppliedFixes);

		analyzerMock.VerifyAll();
		fixerSourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}

	[Fact]
	public static async Task TestThrowsFixerExceptionIfThrowOnFailureIsTrue()
	{
		const string analyzerName = "TestAnalyzer";

		var analyzerMock = new Mock<IAnalyzer>();
		var fixerSourceMock = new Mock<IFixerSource>();
		var analysisResultMock = new Mock<IApplicableAnalysisResult>();
		var sourceUri = new Uri("file:///C:/test.step");

		analyzerMock
			.Setup(a => a.AnalyzeAsync(fixerSourceMock.Object, It.IsAny<CancellationToken>()))
			.ThrowsAsync(new TestAnalysisException())
			.Verifiable();

		analyzerMock
			.SetupGet(a => a.Name)
			.Returns(analyzerName)
			.Verifiable();

		fixerSourceMock
			.SetupGet(f => f.Uri)
			.Returns(sourceUri)
			.Verifiable();

		var fixer = new TestBaseFixer
		{
			ThrowOnFailure = true,
		};

		var ex = await Assert.ThrowsAsync<FixerException>(async () =>
			await fixer.FixAsync(analyzerMock.Object, fixerSourceMock.Object, CancellationToken.None));

		Assert.Equal($"Failed to run analyzer '{analyzerName}' on file '{sourceUri}'", ex.Message);
		_ = Assert.IsType<TestAnalysisException>(ex.InnerException);

		analyzerMock.VerifyAll();
		fixerSourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}
}

file class TestBaseFixer(FixerResult? fixerResult = null) : BaseFixer
{
	protected override Task<FixerResult> ApplyResultAsync(IApplicableAnalysisResult result, IFixerSource source,
		CancellationToken cancellationToken)
	{
		return Task.FromResult(fixerResult ?? FixerResult.None);
	}
}

file class TestAnalysisException : Exception;
