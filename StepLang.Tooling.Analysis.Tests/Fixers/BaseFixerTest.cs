using StepLang.Tooling.Analysis.Analyzers;
using StepLang.Tooling.Analysis.Analyzers.Results;
using StepLang.Tooling.Analysis.Analyzers.Source;
using StepLang.Tooling.Analysis.Fixers;

namespace StepLang.Tooling.Analysis.Tests.Fixers;

public static class BaseFixerTest
{
	[Test]
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

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.AppliedFixes, Is.Zero);
			Assert.That(unfixableInvoked, Is.True);
		}

		analyzerMock.VerifyAll();
		fixerSourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}

	[Test]
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

		Assert.That(result.AppliedFixes, Is.Zero);

		analyzerMock.VerifyAll();
		fixerSourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}

	[Test]
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

		Assert.That(result.AppliedFixes, Is.Zero);

		analyzerMock.VerifyAll();
		fixerSourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}

	[Test]
	public static void TestThrowsFixerExceptionIfThrowOnFailureIsTrue()
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

		var ex = Assert.ThrowsAsync<FixerException>(() =>
				fixer.FixAsync(analyzerMock.Object, fixerSourceMock.Object, CancellationToken.None));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(ex.Message, Is.EqualTo($"Failed to run analyzer '{analyzerName}' on file '{sourceUri}'"));
			Assert.That(ex.InnerException, Is.TypeOf<TestAnalysisException>());
		}

		analyzerMock.VerifyAll();
		fixerSourceMock.VerifyAll();
		analysisResultMock.VerifyAll();
	}
}

file sealed class TestBaseFixer(FixerResult? fixerResult = null) : BaseFixer
{
	protected override Task<FixerResult> ApplyResultAsync(IApplicableAnalysisResult result, IFixerSource source,
		CancellationToken cancellationToken)
	{
		return Task.FromResult(fixerResult ?? FixerResult.None);
	}
}

file sealed class TestAnalysisException : Exception;
