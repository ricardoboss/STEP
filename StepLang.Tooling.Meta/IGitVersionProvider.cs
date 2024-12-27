namespace StepLang.Tooling.Meta;

public interface IGitVersionProvider
{
	string FullSemVer { get; }

	string Sha { get; }

	string BranchName { get; }
}
