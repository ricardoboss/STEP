using System.Diagnostics;
using StepLang.Formatters.Fixers.Results;

namespace StepLang.Formatters.Applicators;

public class DefaultFixApplicator : BaseFixApplicator
{
    protected override async Task<FixApplicatorResult> ApplyResult(FixResult result, FileInfo file,
        CancellationToken cancellationToken)
    {
        Stopwatch sw = new();

        sw.Start();

        switch (result)
        {
            case StringFixResult stringFixResult:
                await File.WriteAllTextAsync(file.FullName, stringFixResult.FixedString, cancellationToken);
                break;
            case FileFixResult fileFixResult:
                fileFixResult.FixedFile.MoveTo(file.FullName, true);
                break;
            default:
                throw new NotImplementedException($"Unknown fix result type '{result.GetType().FullName}'");
        }

        sw.Stop();

        return new(1, 0, sw.Elapsed);
    }
}