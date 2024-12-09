using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting;

public class FixerException(IAnalyzer analyzer, string? message = null, Exception? inner = null)
	: Exception(message, inner)
{
	public IAnalyzer Analyzer { get; } = analyzer;
}
