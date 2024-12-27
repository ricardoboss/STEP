using cmdwtf;
using StepLang.Tooling.Meta;

namespace StepLang.CLI;

internal sealed class CliBuildTimeProvider : IBuildTimeProvider
{
	public static CliBuildTimeProvider Instance { get; } = new();

	private CliBuildTimeProvider() { }

	public DateTimeOffset BuildTime => BuildTimestamp.BuildTimeDto;
}
