using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Spectre.Console;
using StepLang.Tokenizing;
using StepLang.Tooling.Highlighting;

namespace StepLang.CLI;

[ExcludeFromCodeCoverage]
internal static class ErrorHandler
{
    public static void HandleException(Exception e) => AnsiConsole.MarkupLine(FormatError(e));

    private static string FormatError(Exception e)
    {
        return e switch
        {
            StepLangException sle => FormatStepLangException(sle),
            _ => FormatGeneralException(e),
        };
    }

    private static string FormatError(string type, string message) => $"[red]! {type}[/]: {message}";

    private static string FormatGeneralException(Exception e) => FormatError(e.GetType().Name, e.Message + Environment.NewLine + e.StackTrace);

    private static string FormatStepLangException(StepLangException e)
    {
        const int contextLineCount = 4;

        IEnumerable<string> outputLines;

        var exceptionName = $"[white on red bold] {e.ErrorCode}: {e.GetType().Name} [/]{Environment.NewLine}";

        var message = $"{Environment.NewLine}\t{e.Message}{Environment.NewLine}";

        if (e.Location is { } location)
        {
            var sourceCode = location.File.Exists ? File.ReadAllText(location.File.FullName) : "";
            var lines = new Highlighter(ColorScheme.Bright).Highlight(sourceCode).ToArray();
            var contextStartLine = Math.Max(0, location.Line - 1 - contextLineCount);
            var contextEndLine = Math.Min(lines.Length, location.Line + contextLineCount);
            var lineNumber = contextStartLine;
            var lineNumberWidth = contextEndLine.ToString(CultureInfo.InvariantCulture).Length;
            var contextLines = lines[contextStartLine..contextEndLine].Select(tokens =>
            {
                var lineContents = tokens.Aggregate("", (current, t) =>
                {
                    var f = $"[#{t.Style.Foreground.ToArgb() & 0xFFFFFF:X6}]";

                    if (t.Type is TokenType.LiteralString)
                        f += "\"";

                    f += t.Text;

                    if (t.Type is TokenType.LiteralString)
                        f += "\"";

                    f += "[/]";

                    return current + f;
                });

                var prefix = lineNumber == location.Line - 1 ? "[red]>[/] " : "  ";

                var displayLineNumber = (lineNumber + 1).ToString(CultureInfo.InvariantCulture);
                var line = prefix + $"[grey]{displayLineNumber.PadLeft(lineNumberWidth)} |[/] {lineContents}";

                lineNumber++;

                return line;
            });

            var locationString = $"at [green]{location.File.FullName}[/]:{location.Line}";

            outputLines = contextLines.Prepend(locationString);
        }
        else
            outputLines = new[] { $"[grey]{e.StackTrace ?? string.Empty}[/]" };

        outputLines = outputLines.Prepend(message).Prepend(exceptionName);

        if (e.HelpText is { } helpText)
            outputLines = outputLines.Append($"{Environment.NewLine}[blue]Tip[/]: {helpText}");

        if (e.HelpLink is { } helpLink)
            outputLines = outputLines.Append($"{Environment.NewLine}[blue]See also[/]: [link]{helpLink}[/]");

        return Environment.NewLine + string.Join(Environment.NewLine, outputLines);
    }
}