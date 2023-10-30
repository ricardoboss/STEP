using System.Collections;
using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.AnalyzerSet;

/// <summary>
/// Represents a set/collection of analyzers that can be enumerated.
/// </summary>
public abstract class BaseAnalyzerSet : IAnalyzerSet
{
    /// <inheritdoc />
    public abstract IEnumerator<IAnalyzer> GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}