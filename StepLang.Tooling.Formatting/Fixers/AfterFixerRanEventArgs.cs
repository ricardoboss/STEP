using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Fixers;

public class AfterFixerRanEventArgs(FileInfo File, IAnalyzer Analyzer) : EventArgs
{
	public FileInfo File { get; } = File;

	public IAnalyzer Analyzer { get; } = Analyzer;
}
