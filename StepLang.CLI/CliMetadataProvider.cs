using StepLang.Tooling.Meta;

namespace StepLang.CLI;

internal sealed class CliMetadataProvider : IMetadataProvider
{
	public static CliMetadataProvider Instance { get; } = new();

	private CliMetadataProvider() { }

	public DateTimeOffset BuildTime => BuildMetadata.BuildTimestamp;

	public string FullSemVer => GitVersionInformation.FullSemVer;

	public string Sha => GitVersionInformation.Sha;

	public string BranchName => GitVersionInformation.BranchName;
}
