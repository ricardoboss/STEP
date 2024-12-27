using StepLang.Tooling.Meta;

namespace StepLang;

public sealed class CoreGitVersionProvider : IGitVersionProvider
{
	public static CoreGitVersionProvider Instance { get; } = new();

	private CoreGitVersionProvider() { }

	public string FullSemVer => GitVersionInformation.FullSemVer;

	public string Sha => GitVersionInformation.Sha;

	public string BranchName => GitVersionInformation.BranchName;
}
