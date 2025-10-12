namespace StepLang.Tooling.Analysis.Fixers;

public record FixerResult
{
	private FixerResult(int AppliedFixes, TimeSpan Elapsed)
	{
		this.AppliedFixes = AppliedFixes;
		this.Elapsed = Elapsed;
	}

	public static FixerResult None { get; } = new(0, TimeSpan.Zero);

	public static FixerResult Applied(TimeSpan elapsed)
	{
		return new FixerResult(1, elapsed);
	}

	public static FixerResult NotApplied(TimeSpan elapsed)
	{
		return new FixerResult(0, elapsed);
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

	public static FixerResult Add(FixerResult left, FixerResult right)
	{
		return left + right;
	}
}
