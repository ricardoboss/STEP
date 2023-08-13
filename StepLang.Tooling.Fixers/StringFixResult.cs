namespace StepLang.Formatters;

public record StringFixResult(bool Success, string? Message, string? FixedString) : FixResult(Success, Message);