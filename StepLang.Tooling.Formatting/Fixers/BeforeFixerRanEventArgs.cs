using StepLang.Tooling.Formatting.Analyzers;
using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Fixers;

public class BeforeFixerRanEventArgs(IFixerSource source, IAnalyzer analyzer) : EventArgs
{
	public IFixerSource Source { get; } = source;

	public IAnalyzer Analyzer { get; } = analyzer;
}
