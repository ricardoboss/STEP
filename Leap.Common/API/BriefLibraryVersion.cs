namespace Leap.Common.API;

public record BriefLibraryVersion(string Author, string Name, string Version, IDictionary<string, string> Dependencies, string DownloadUrl);
