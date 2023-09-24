using System.Globalization;
using Spectre.Console.Rendering;
using StepLang.CLI.Extensions;
using StepLang.Tokenizing;
using StepLang.Tooling.Formatting;
using StepLang.Tooling.Highlighting;
using Style = Spectre.Console.Style;

namespace StepLang.CLI.Widgets;

internal sealed class Code : IRenderable
{
    private const int LineNumberPaddingWidth = 1;
    private const int LineNumberSeparatorWidth = 1;
    private const string AsciiLineNumberSeparator = "|";
    private const string UnicodeLineNumberSeparator = "\u2502";

    private const int LineMarkerPaddingWidth = 1;
    private const int LineMarkerWidth = 1;
    private const string AsciiLineMarker = ">";
    private const string UnicodeLineMarker = "\u25B6";

    private readonly string code;
    private readonly bool showLineNumbers;
    private readonly int lineNumberOffset;
    private readonly int? markedLineNumber;
    private readonly ColorScheme scheme;

    private List<string>? lines;
    private List<string> Lines => lines ??= code.SplitLines().ToList();

    public Code(string code, ColorScheme scheme, bool showLineNumbers = true, int lineNumberOffset = 0,
        int? markedLineNumber = null)
    {
        if (lineNumberOffset < 0)
            throw new ArgumentOutOfRangeException(nameof(lineNumberOffset), "Line number offset cannot be negative.");

        this.code = code;
        this.showLineNumbers = showLineNumbers;
        this.scheme = scheme;
        this.lineNumberOffset = lineNumberOffset;
        this.markedLineNumber = markedLineNumber;
    }

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        var max = Lines.Max(l => l.Length);

        if (showLineNumbers)
        {
            var lineNumberWidth = Lines.Count.ToString(CultureInfo.InvariantCulture).Length;

            max += lineNumberWidth + LineNumberPaddingWidth + LineNumberSeparatorWidth + LineNumberPaddingWidth;
        }

        if (markedLineNumber is not null)
        {
            max += LineMarkerWidth + LineMarkerPaddingWidth;
        }

        return new(max, max);
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var highlighter = new Highlighter(scheme);
        var highlighted = highlighter.Highlight(code).ToList();

        var currentLineNumber = lineNumberOffset;
        var lastLineNumber = currentLineNumber + highlighted.Count;
        var lastLineNumberWidth = lastLineNumber.ToString(CultureInfo.InvariantCulture).Length;

        var defaultStyle = scheme.Default.ToSpectreStyle();
        var markerStyle = Style.Parse("red");

        var lineNumberSeparator = options.Unicode ? UnicodeLineNumberSeparator : AsciiLineNumberSeparator;
        var lineMarker = options.Unicode ? UnicodeLineMarker : AsciiLineMarker;

        using var enumerator = highlighted.GetEnumerator();
        while (enumerator.MoveNext())
        {
            currentLineNumber++;

            if (markedLineNumber is not null)
            {
                if (markedLineNumber == currentLineNumber)
                    yield return new(lineMarker, markerStyle);
                else
                    yield return new(new(' ', LineMarkerWidth), defaultStyle);

                yield return new(new(' ', LineMarkerPaddingWidth), defaultStyle);
            }

            if (showLineNumbers)
            {
                yield return new(currentLineNumber.ToString(CultureInfo.InvariantCulture).PadLeft(lastLineNumberWidth),
                    defaultStyle);

                yield return new(new(' ', LineMarkerWidth), defaultStyle);
                yield return new(lineNumberSeparator, defaultStyle);
                yield return new(new(' ', LineMarkerWidth), defaultStyle);
            }

            while (enumerator.Current is not null && enumerator.Current.Type != TokenType.NewLine)
            {
                yield return new(enumerator.Current.Text.Replace("\t", "    "), enumerator.Current.Style.ToSpectreStyle());

                enumerator.MoveNext();
            }

            yield return Segment.LineBreak;
        }
    }
}