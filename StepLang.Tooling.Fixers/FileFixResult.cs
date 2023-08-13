namespace StepLang.Formatters;

public record FileFixResult(bool Success, string? Message, FileInfo? FixedFile) : FixResult(Success, Message);