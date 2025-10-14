using StepLang.Tooling.Diagnostics.Analyzers;

namespace StepLang.Tooling.Diagnostics;

public interface IDiagnosticsAnalyzerProvider
{
	IEnumerable<IDiagnosticsAnalyzer> GetAnalyzers();
}
