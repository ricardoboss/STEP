namespace StepLang.Tooling.CLI;

public interface IBuildTimeProvider
{
	DateTimeOffset BuildTimeUtc { get; }
}
