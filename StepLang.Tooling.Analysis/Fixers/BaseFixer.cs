using StepLang.Tooling.Analysis.Analyzers;
using StepLang.Tooling.Analysis.Analyzers.Results;
using StepLang.Tooling.Analysis.Analyzers.Source;
using System.Diagnostics;

namespace StepLang.Tooling.Analysis.Fixers;

public abstract class BaseFixer : IFixer
{
	private const string DefaultSearchPattern = "*.step";

	public virtual bool ThrowOnFailure { get; init; } = true;

	public event EventHandler<BeforeFixerRanEventArgs>? OnCheck;

	public event EventHandler<UnfixableEventArgs>? OnUnfixable;

	public event EventHandler<AfterFixerRanEventArgs>? OnFixed;

	public async Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, DirectoryInfo directory, CancellationToken cancellationToken = default)
	{
		var analyzerList = analyzers.ToList();

		return await directory
			.EnumerateFiles(DefaultSearchPattern, SearchOption.AllDirectories)
			.ToAsyncEnumerable()
			.AggregateAwaitAsync(
				FixerResult.None,
				async (result, file) => result + await FixAsync(analyzerList, file, cancellationToken),
				cancellationToken
			);
	}

	public async Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, FileInfo file, CancellationToken cancellationToken = default)
	{
		var source = new FileFixerSource(file);

		return await analyzers
			.ToAsyncEnumerable()
			.AggregateAwaitAsync(
				FixerResult.None,
				async (result, analyzer) => result + await FixAsync(analyzer, source, cancellationToken),
				cancellationToken
			);
	}

	public virtual async Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, IFixerSource source,
		CancellationToken cancellationToken = default)
	{
		return await analyzers
			.ToAsyncEnumerable()
			.AggregateAwaitAsync(
				FixerResult.None,
				async (result, analyzer) => result + await FixAsync(analyzer, source, cancellationToken),
				cancellationToken
			);
	}

	public virtual async Task<FixerResult> FixAsync(IAnalyzer analyzer, IFixerSource source,
		CancellationToken cancellationToken = default)
	{
		OnCheck?.Invoke(this, new BeforeFixerRanEventArgs(source, analyzer));

		Stopwatch sw = new();

		sw.Start();

		IAnalysisResult analysisResult;
		try
		{
			analysisResult = await analyzer.AnalyzeAsync(source, cancellationToken);

			sw.Stop();
		}
		catch (Exception e)
		{
			sw.Stop();

			if (ThrowOnFailure)
			{
				throw new FixerException(analyzer,
					$"Failed to run analyzer '{analyzer.Name}' on file '{source.Uri}'", e);
			}

			return FixerResult.NotApplied(sw.Elapsed);
		}

		var analysisDuration = sw.Elapsed;

		if (!analysisResult.ShouldFix)
			return FixerResult.NotApplied(analysisDuration);

		if (analysisResult is not IApplicableAnalysisResult { FixAvailable: true } applicableAnalysisResult)
		{
			OnUnfixable?.Invoke(this, new UnfixableEventArgs(source, analyzer));

			return FixerResult.NotApplied(analysisDuration);
		}

		var result = await ApplyResultAsync(applicableAnalysisResult, source, cancellationToken);

		OnFixed?.Invoke(this, new AfterFixerRanEventArgs(source, analyzer));

		return result with { Elapsed = result.Elapsed + analysisDuration };
	}

	protected abstract Task<FixerResult> ApplyResultAsync(IApplicableAnalysisResult result, IFixerSource source,
		CancellationToken cancellationToken);
}
