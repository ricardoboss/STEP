using StepLang.Tooling.Meta;

namespace StepLang.CLI;

internal sealed class CliGitVersionProvider : IGitVersionProvider
{
	public static CliGitVersionProvider Instance { get; } = new();

	private CliGitVersionProvider() { }

	public string FullSemVer => GitVersionInformation.FullSemVer;
	public string Sha => GitVersionInformation.Sha;
	public string BranchName => GitVersionInformation.BranchName;
}
