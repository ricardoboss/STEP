using StepLang.Tooling.Meta;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.CLI;

[ExcludeFromCodeCoverage]
internal sealed class CliMetadataProvider : IMetadataProvider
{
	public static CliMetadataProvider Instance { get; } = new();

	private CliMetadataProvider() { }

	public DateTimeOffset BuildTime => BuildMetadata.BuildTimestamp;

	public string FullSemVer => GitVersionInformation.FullSemVer;

	public string Sha => GitVersionInformation.Sha;

	public string BranchName => GitVersionInformation.BranchName;
}
