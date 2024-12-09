using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Fixers;

public class BeforeFixerRanEventArgs(FileInfo File, IAnalyzer Analyzer) : EventArgs
{
	public FileInfo File { get; } = File;
	public IAnalyzer Analyzer { get; } = Analyzer;
}
