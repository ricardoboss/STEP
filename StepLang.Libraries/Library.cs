namespace StepLang.Libraries;

public record Library(string Name, string? Version, string? Author, IEnumerable<Dependency>? Dependencies, IEnumerable<string>? Files);
