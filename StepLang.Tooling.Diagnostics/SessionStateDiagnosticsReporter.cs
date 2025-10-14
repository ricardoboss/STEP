using StepLang.Diagnostics;
using StepLang.Tooling.Diagnostics.Analyzers;

namespace StepLang.Tooling.Diagnostics;

public class SessionStateDiagnosticsReporter(SessionState sessionState) : IDiagnosticsReporter
{
	public Task ReportAsync(DocumentState documentState, Diagnostic diagnostic,
		CancellationToken cancellationToken = default)
	{
		sessionState.Diagnostics[documentState.DocumentUri].Add(diagnostic);

		return Task.CompletedTask;
	}
}
