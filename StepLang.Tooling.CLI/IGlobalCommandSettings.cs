namespace StepLang.Tooling.CLI;

public interface IGlobalCommandSettings
{
	bool Info { get; }

	bool Version { get; }

	bool Handled { get; set; }
}
