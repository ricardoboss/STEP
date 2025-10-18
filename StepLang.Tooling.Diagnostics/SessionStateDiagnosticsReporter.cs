using StepLang.Diagnostics;
using StepLang.Tooling.Diagnostics.Analyzers;

namespace StepLang.Tooling.Diagnostics;

public class SessionStateDiagnosticsReporter(DiagnosticsSessionState diagnosticsSessionState) : IDiagnosticsReporter
{
	public Task ReportAsync(DocumentState documentState, Diagnostic diagnostic,
		CancellationToken cancellationToken = default)
	{
		diagnosticsSessionState.Diagnostics[documentState.DocumentUri].Add(diagnostic);

		return Task.CompletedTask;
	}
}
