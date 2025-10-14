namespace StepLang.Tooling.Diagnostics.Analyzers;

public interface IDiagnosticsAnalyzer
{
	Task AnalyzeAsync(DocumentState state, IDiagnosticsReporter reporter,
		CancellationToken cancellationToken = default);
}
