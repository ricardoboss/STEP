using Spectre.Console.Cli;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using StepLangCliTelemetry = StepLang.Tooling.CLI.Telemetry;

namespace StepLang.Tooling.CLI;

public class TelemetryInterceptor : ICommandInterceptor
{
	private static readonly Histogram<double> RuntimeHistogram =
		StepLangCliTelemetry.CommandMeter.CreateHistogram<double>("Command.Runtime");

	private Activity? activitySpan;

	public void Intercept(CommandContext context, CommandSettings settings)
	{
		activitySpan?.Dispose();

		activitySpan = StepLangCliTelemetry.Activity.StartActivity("Command");
		activitySpan?.SetTag("Name", context.Name);
		activitySpan?.SetTag("Arguments", context.Arguments);
	}

	public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
	{
		activitySpan?.Stop();

		var runtime = activitySpan?.Duration;

		if (runtime is { TotalMilliseconds: var runtimeMillis })
			RuntimeHistogram.Record(runtimeMillis);

		activitySpan?.Dispose();
	}
}
