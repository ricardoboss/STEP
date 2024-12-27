namespace StepLang.Tooling.Meta;

public interface IMetadataProvider
{
	DateTimeOffset BuildTime { get; }

	string FullSemVer { get; }

	string Sha { get; }

	string BranchName { get; }
}
