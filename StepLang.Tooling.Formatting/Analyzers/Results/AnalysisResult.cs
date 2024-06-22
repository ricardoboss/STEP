namespace StepLang.Tooling.Formatting.Analyzers.Results;

/// <summary>
/// Base class for all analysis results.
/// </summary>
/// <param name="FixRequired">Whether a fix is required.</param>
public abstract record AnalysisResult(bool FixRequired);
