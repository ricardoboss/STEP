namespace StepLang.Tooling.Meta;

public interface IBuildTimeProvider
{
	DateTimeOffset BuildTime { get; }
}
