namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// An analyzer that can be used to analyze files and source code.
/// </summary>
public interface IAnalyzer
{
    /// <summary>
    /// The name of the analyzer.
    /// </summary>
    public string Name => GetType().Name;
}