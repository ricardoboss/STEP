using Microsoft.Extensions.Logging;
using StepLang.Tooling.Diagnostics.Analyzers;

namespace StepLang.Tooling.Diagnostics;

public sealed class DefaultDiagnosticsRunner(
	IDiagnosticsAnalyzerProvider analyzerProvider,
	ILogger<DefaultDiagnosticsRunner> logger
) : IDiagnosticsRunner
{
	public async Task RunDiagnosticsAsync(DocumentState document, IDiagnosticsReporter reporter,
		CancellationToken cancellationToken = default)
	{
		logger.LogDebug("Running diagnostics for document {@Document}", document);

		await Task.WhenAll(
			analyzerProvider.GetAnalyzers().Select(analyzer =>
			{
				logger.LogTrace("Running analyzer \"{Analyzer}\" on document {@Document}", analyzer.GetType().Name,
					document);

				return analyzer.AnalyzeAsync(document, reporter, CancellationToken.None);
			})
		);

		logger.LogDebug("Finished running diagnostics for document {@Document}", document);
	}
}
