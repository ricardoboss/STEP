using StepLang.Formatters.Fixers.Results;

namespace StepLang.Formatters.Applicators;

public class DryRunFixApplicator : BaseFixApplicator
{
    protected override Task<FixApplicatorResult> ApplyResult(FixResult result, FileInfo file,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<FixApplicatorResult>(new(1, 0, TimeSpan.Zero));
    }
}