using StepLang.Diagnostics;

namespace StepLang.Tooling.Diagnostics.Analyzers;

public interface IDiagnosticsReporter
{
	Task ReportAsync(DocumentState documentState, Diagnostic diagnostic, CancellationToken cancellationToken = default);
}
