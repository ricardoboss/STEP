namespace StepLang.Formatters;

public abstract record FixResult(bool Success, string? Message = null);