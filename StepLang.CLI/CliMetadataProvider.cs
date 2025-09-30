using StepLang.Tooling.Meta;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.CLI;

[ExcludeFromCodeCoverage]
internal sealed class CliMetadataProvider : IMetadataProvider
{
	public static CliMetadataProvider Instance { get; } = new();

	private CliMetadataProvider() { }

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
