namespace StepLang.Tooling.CLI;

public interface IGitVersionProvider
{
	string FullSemVer { get; }

	string Sha { get; }

	string BranchName { get; }
}
