using StepLang.Tooling.Analysis.Analyzers;
using StepLang.Tooling.Analysis.Analyzers.Source;

namespace StepLang.Tooling.Analysis.Fixers;

public class UnfixableEventArgs(IFixerSource source, IAnalyzer analyzer) : EventArgs
{
	public IFixerSource Source { get; } = source;

	public IAnalyzer Analyzer { get; } = analyzer;
}
