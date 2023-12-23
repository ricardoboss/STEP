using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Fixers;

/// <summary>
/// Event arguments for when a fixer has applied fixes from a single <see cref="IAnalyzer"/> to a file.
/// </summary>
/// <param name="File">The file that was fixed.</param>
/// <param name="Analyzer">The analyzer that was used to fix the file.</param>
public record AfterFixerRanEventArgs(FileInfo File, IAnalyzer Analyzer);
