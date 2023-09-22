namespace StepLang.Libraries;

public record Library(string Name, string? Version, string? Author, Dictionary<string, string>? Dependencies, List<string>? Files);
