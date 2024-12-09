namespace StepLang.CLI.Commands;

internal interface IGlobalCommandSettings
{
	public const string InfoOptionName = "--info";
	public const string InfoOptionDescription =
		"Print the version and system information. Add the output of this to bug reports.";
	public const bool InfoOptionDefaultValue = false;
	public const string VersionOptionName = "-v|--version";
	public const string VersionOptionDescription = "Print the version number.";
	public const bool VersionOptionDefaultValue = false;

	public bool Info { get; }

	public bool Version { get; }
}
