namespace StepLang.Formatters.Fixers.Results;

public record FileFixResult(bool FixRequired, FileInfo FixedFile) : FixResult(FixRequired);