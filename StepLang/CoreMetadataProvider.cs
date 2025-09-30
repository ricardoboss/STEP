using StepLang.Tooling.Meta;
using System.Diagnostics.CodeAnalysis;

namespace StepLang;

[ExcludeFromCodeCoverage]
public sealed class CoreMetadataProvider : IMetadataProvider
{
	public static CoreMetadataProvider Instance { get; } = new();

	private CoreMetadataProvider() { }

	public DateTimeOffset BuildTime => BuildMetadata.BuildTimestamp;

#if RELEASE
	public string FullSemVer => GitVersionInformation.FullSemVer;

	public string Sha => GitVersionInformation.Sha;

	public string BranchName => GitVersionInformation.BranchName;
#else
	public string FullSemVer => "99.99.99";

	public string Sha => "01234567";

	public string BranchName => "dev";
#endif
}
