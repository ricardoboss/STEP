namespace StepLang.Tooling.Formatting.Fixers;

public record FixerResult
{
	protected FixerResult(int AppliedFixes, TimeSpan Elapsed)
	{
		this.AppliedFixes = AppliedFixes;
		this.Elapsed = Elapsed;
	}

	public static FixerResult None { get; } = new(0, TimeSpan.Zero);

	public static FixerResult Applied(int count, TimeSpan elapsed)
	{
		return new FixerResult(count, elapsed);
	}

	public int AppliedFixes { get; init; }
	public TimeSpan Elapsed { get; init; }

	public static FixerResult operator +(FixerResult a, FixerResult b)
	{
		return new FixerResult(
			a.AppliedFixes + b.AppliedFixes,
			a.Elapsed + b.Elapsed
		);
	}
}
