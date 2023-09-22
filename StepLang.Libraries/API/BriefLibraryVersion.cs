namespace StepLang.Libraries.API;

public record BriefLibraryVersion(string Name, string Version, IEnumerable<string> Dependencies, string DownloadUrl);
