using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Fixers;

public record BeforeFixerRanEventArgs(FileInfo File, IAnalyzer Analyzer);
