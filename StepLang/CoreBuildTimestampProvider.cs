using cmdwtf;
using StepLang.Tooling.Meta;

namespace StepLang;

public sealed class CoreBuildTimestampProvider : IBuildTimeProvider
{
	public static CoreBuildTimestampProvider Instance { get; } = new();

	private CoreBuildTimestampProvider() { }

	public DateTimeOffset BuildTime => BuildTimestamp.BuildTimeDto;
}
