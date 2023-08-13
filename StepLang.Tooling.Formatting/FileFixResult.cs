namespace StepLang.Formatters;

public record FileFixResult(bool FixRequired, FileInfo FixedFile) : FixResult(FixRequired);