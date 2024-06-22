namespace StepLang.Tooling.Formatting.Fixers;

/// <summary>
/// Represents the result of a fixer.
/// </summary>
public record FixerResult
{
    private FixerResult(int AppliedFixes, TimeSpan Elapsed)
    {
        this.AppliedFixes = AppliedFixes;
        this.Elapsed = Elapsed;
    }

    /// <summary>
    /// A result that indicates that no fixes were applied and that no time was spent.
    /// </summary>
    public static FixerResult None { get; } = new(0, TimeSpan.Zero);

    /// <summary>
    /// Creates a result that indicates that <paramref name="count"/> fixes were applied and that <paramref name="elapsed"/> time was spent.
    /// </summary>
    /// <param name="count">The number of fixes that were applied.</param>
    /// <param name="elapsed">The amount of time that was spent applying the fixes.</param>
    /// <returns>A result that indicates that <paramref name="count"/> fixes were applied and that <paramref name="elapsed"/> time was spent.</returns>
    public static FixerResult Applied(int count, TimeSpan elapsed) => new(count, elapsed);

    /// <summary>
    /// The number of fixes that were applied.
    /// </summary>
    public int AppliedFixes { get; init; }

    /// <summary>
    /// The amount of time that was spent applying the fixes.
    /// </summary>
    public TimeSpan Elapsed { get; init; }

    /// <summary>
    /// Combines two results into a single result. Adds the number of applied fixes and the elapsed time.
    /// </summary>
    /// <param name="a">The first result.</param>
    /// <param name="b">The second result.</param>
    /// <returns>A result that combines the two results.</returns>
    public static FixerResult operator +(FixerResult a, FixerResult b)
    {
        return new(
            a.AppliedFixes + b.AppliedFixes,
            a.Elapsed + b.Elapsed
        );
    }
}