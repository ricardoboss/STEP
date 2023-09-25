namespace Leap.Common.API;

public record BriefLibraryVersion(string Name, string Version, IEnumerable<string> Dependencies, string DownloadUrl);
