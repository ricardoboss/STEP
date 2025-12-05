using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace StepLang.Tooling.CLI;

public static class Telemetry
{
	public const string ActivityName = "StepLang.CLI";

	public static readonly ActivitySource Activity = new(ActivityName);

	public const string CommandMeterName = "Command";

	public static readonly Meter CommandMeter = new Meter(CommandMeterName);
}
