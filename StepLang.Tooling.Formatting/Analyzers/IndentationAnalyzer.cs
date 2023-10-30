using System.Text;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// Checks the indentation of code blocks and provides a fix if it is not correct.
/// </summary>
public class IndentationAnalyzer : IStringAnalyzer
{
    /// <inheritdoc />
    public Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        StringBuilder builder = new();
        var currentIndent = 0;
        foreach (var (lineEnding, line) in input.SplitLinesPreserveNewLines())
        {
            var trimmedLine = line.TrimStart();
            if (trimmedLine.StartsWith("}") && currentIndent > 0)
            {
                currentIndent--;
            }

            if (trimmedLine.Length > 0)
            {
                builder.Append(new string('\t', currentIndent));
                builder.Append(trimmedLine);
            }

            builder.Append(lineEnding);

            if (trimmedLine.TrimEnd().EndsWith("{"))
            {
                currentIndent++;
            }
        }

        return Task.FromResult(StringAnalysisResult.FromInputAndFix(input, builder.ToString()));
    }
}