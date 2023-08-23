using System.Text;
using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class IndentationFixer : IStringFixer
{
    public Task<StringFixResult> FixAsync(string input, CancellationToken cancellationToken = default)
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

            builder.Append(new string('\t', currentIndent));
            builder.Append(trimmedLine);
            builder.Append(lineEnding);

            if (trimmedLine.TrimEnd().EndsWith("{"))
            {
                currentIndent++;
            }
        }

        return Task.FromResult(StringFixResult.FromInputAndFix(input, builder.ToString()));
    }
}