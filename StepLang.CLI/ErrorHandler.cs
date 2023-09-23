using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Spectre.Console;

namespace StepLang.CLI;

[ExcludeFromCodeCoverage]
internal static class ErrorHandler
{
    public static void HandleException(Exception e) => AnsiConsole.MarkupLineInterpolated(FormatError(e));

    private static FormattableString FormatError(Exception e)
    {
        return e switch
        {
            StepLangException sle => FormatStepLangException(sle),
            _ => FormatGeneralException(e),
        };
    }

    private static FormattableString FormatError(string type, string message) => $"[red]! {type}[/]: {message}";

    private static FormattableString FormatGeneralException(Exception e) => FormatError(e.GetType().Name, e.Message + Environment.NewLine + e.StackTrace);

    private static FormattableString FormatStepLangException(StepLangException e)
    {
        const int contextLineCount = 4;

        IEnumerable<string> outputLines;

        var exceptionName = " " + e.ErrorCode + ": " + e.GetType().Name + " ";

        var message = Environment.NewLine + "\t" + e.Message + Environment.NewLine;

        if (e.Location is { } location)
        {
            var sourceCode = location.File.Exists ? File.ReadAllText(location.File.FullName) : "";
            var lines = sourceCode.ReplaceLineEndings().Split(Environment.NewLine);
            var contextStartLine = Math.Max(0, location.Line - 1 - contextLineCount);
            var contextEndLine = Math.Min(lines.Length, location.Line + contextLineCount);
            var lineNumber = contextStartLine;
            var lineNumberWidth = contextEndLine.ToString(CultureInfo.InvariantCulture).Length;
            var contextLines = lines[contextStartLine..contextEndLine].Select(l =>
            {
                var prefix = lineNumber == location.Line - 1 ? "> " : "  ";

                var displayLineNumber = (lineNumber + 1).ToString(CultureInfo.InvariantCulture);
                var line = prefix + $"{displayLineNumber.PadLeft(lineNumberWidth) + "|"} {l}";

                lineNumber++;

                return line;
            });

            var locationString = $"at {location.File.FullName}:{location.Line}";

            outputLines = contextLines.Prepend(locationString);
        }
        else
            outputLines = new[] { e.StackTrace ?? string.Empty };

        outputLines = outputLines.Prepend(message).Prepend(exceptionName);

        if (e.HelpText is { } helpText)
            outputLines = outputLines.Append(Environment.NewLine + "Tip: " + helpText);

        if (e.HelpLink is { } helpLink)
            outputLines = outputLines.Append(Environment.NewLine + "See also: " + helpLink);

        return $"{Environment.NewLine}{string.Join(Environment.NewLine, outputLines)}";
    }
}