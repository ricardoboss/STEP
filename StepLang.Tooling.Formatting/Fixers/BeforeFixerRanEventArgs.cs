using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Fixers;

/// <summary>
/// Event arguments for <see cref="IFixer.OnCheck"/>.
/// </summary>
/// <param name="File">The file that is about to be fixed.</param>
/// <param name="Analyzer">The analyzer that is about to be applied.</param>
public record BeforeFixerRanEventArgs(FileInfo File, IAnalyzer Analyzer);
