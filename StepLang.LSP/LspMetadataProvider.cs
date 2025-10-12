using StepLang.Tooling.Meta;

namespace StepLang.LSP;

internal sealed class LspMetadataProvider : IMetadataProvider
{
	public static LspMetadataProvider Instance { get; } = new();

	private LspMetadataProvider() { }

	public DateTimeOffset BuildTime => BuildMetadata.BuildTimestamp;

#if RELEASE && !TEST
	public string FullSemVer => GitVersionInformation.FullSemVer;

	public string Sha => GitVersionInformation.Sha;

	public string BranchName => GitVersionInformation.BranchName;
#else
	public string FullSemVer => "99.99.99";

	public string Sha => "01234567";

	public string BranchName => "dev";
#endif
}
