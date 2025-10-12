namespace StepLang.Tooling.Diagnostics.Analyzers;

public interface IDiagnosticsAnalyzer
{
	string Name => GetType().Name;

	Task AnalyzeAsync(SessionState sessionState, DocumentState documentState,
		CancellationToken cancellationToken = default);
}
