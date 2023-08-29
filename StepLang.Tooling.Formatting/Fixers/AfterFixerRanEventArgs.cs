using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Fixers;

public record AfterFixerRanEventArgs(FileInfo File, IAnalyzer Analyzer);