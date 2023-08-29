using System.Diagnostics;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class DefaultFixer : BaseFixer
{
    protected override async Task<FixerResult> ApplyResult(AnalysisResult result, FileInfo file,
        CancellationToken cancellationToken)
    {
        Stopwatch sw = new();

        sw.Start();

        switch (result)
        {
            case StringAnalysisResult stringFixResult:
                await File.WriteAllTextAsync(file.FullName, stringFixResult.FixedString, cancellationToken);
                break;
            case FileAnalysisResult fileFixResult:
                fileFixResult.FixedFile.MoveTo(file.FullName, true);
                break;
            default:
                throw new NotImplementedException($"Unknown fix result type '{result.GetType().FullName}'");
        }

        sw.Stop();

        return FixerResult.Applied(1, sw.Elapsed);
    }
}