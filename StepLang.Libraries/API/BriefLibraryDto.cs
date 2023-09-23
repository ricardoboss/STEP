namespace StepLang.Libraries.API;

public record BriefLibraryDto(string Name, VersionDto Version, IEnumerable<string> Dependencies);
