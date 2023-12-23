using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.AnalyzerSet;

/// <summary>
/// Represents a set/collection of analyzers that can be enumerated.
/// </summary>
public interface IAnalyzerSet : IEnumerable<IAnalyzer>;
