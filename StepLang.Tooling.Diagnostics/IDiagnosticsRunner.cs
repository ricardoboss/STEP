using StepLang.Tooling.Diagnostics.Analyzers;

namespace StepLang.Tooling.Diagnostics;

public interface IDiagnosticsRunner
{
	Task RunDiagnosticsAsync(DocumentState document, IDiagnosticsReporter reporter,
		CancellationToken cancellationToken = default);
}
