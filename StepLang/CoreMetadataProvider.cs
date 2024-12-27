using cmdwtf;
using StepLang.Tooling.Meta;

namespace StepLang;

public sealed class CoreMetadataProvider : IMetadataProvider
{
	public static CoreMetadataProvider Instance { get; } = new();

	private CoreMetadataProvider() { }

	public DateTimeOffset BuildTime => BuildTimestamp.BuildTimeDto;

	public string FullSemVer => GitVersionInformation.FullSemVer;

	public string Sha => GitVersionInformation.Sha;

	public string BranchName => GitVersionInformation.BranchName;
}
