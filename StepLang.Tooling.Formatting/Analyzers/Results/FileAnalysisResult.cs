namespace StepLang.Tooling.Formatting.Analyzers.Results;

public record FileAnalysisResult(bool FixRequired, FileInfo FixedFile) : AnalysisResult(FixRequired);