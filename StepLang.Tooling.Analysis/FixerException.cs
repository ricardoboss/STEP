using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis;

public class FixerException(IAnalyzer analyzer, string? message = null, Exception? inner = null)
	: Exception(message, inner)
{
	public IAnalyzer Analyzer { get; } = analyzer;
}
