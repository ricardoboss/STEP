namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// This is just a marker for all fixers.
/// </summary>
public interface IAnalyzer
{
    public string Name => GetType().Name;
}