using StepLang.Tooling.Meta;
using System.Diagnostics.CodeAnalysis;

namespace StepLang;

[ExcludeFromCodeCoverage]
public sealed class CoreMetadataProvider : IMetadataProvider
{
	public static CoreMetadataProvider Instance { get; } = new();

	private CoreMetadataProvider() { }

	public DateTimeOffset BuildTime => BuildMetadata.BuildTimestamp;

	public string FullSemVer => GitVersionInformation.FullSemVer;

	public string Sha => GitVersionInformation.Sha;

	public string BranchName => GitVersionInformation.BranchName;
}
