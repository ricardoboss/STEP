namespace StepLang.Formatters.Applicators;

public record FixApplicatorResult(int AppliedFixers, int FailedFixers, TimeSpan Elapsed) {
    public static FixApplicatorResult Zero { get; } = new(0, 0, TimeSpan.Zero);

    public static FixApplicatorResult operator +(FixApplicatorResult a, FixApplicatorResult b)
    {
        return new(
            a.AppliedFixers + b.AppliedFixers,
            a.FailedFixers + b.FailedFixers,
            a.Elapsed + b.Elapsed
        );
    }

    public int TotalFixers => AppliedFixers + FailedFixers;
}