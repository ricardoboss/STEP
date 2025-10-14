using Microsoft.Extensions.DependencyInjection;
using StepLang.Tooling.Diagnostics.Analyzers;

namespace StepLang.Tooling.Diagnostics;

public class ServiceProviderDiagnosticsAnalyzerProvider(IServiceProvider serviceProvider) : IDiagnosticsAnalyzerProvider
{
	private readonly Lazy<IEnumerable<IDiagnosticsAnalyzer>> lazyAnalyzers =
		new(serviceProvider.GetServices<IDiagnosticsAnalyzer>);

	public IEnumerable<IDiagnosticsAnalyzer> GetAnalyzers() => lazyAnalyzers.Value;
}
