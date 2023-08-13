namespace StepLang.Tooling.Formatting.Fixers.Results;

public record FileFixResult(bool FixRequired, FileInfo FixedFile) : FixResult(FixRequired);