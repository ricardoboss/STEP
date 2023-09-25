namespace Leap.Common.API;

public record BriefLibraryVersion(string Author, string Name, string Version, IEnumerable<string> Dependencies, string DownloadUrl);
