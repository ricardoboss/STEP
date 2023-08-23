using System.Text;

namespace StepLang.Tooling.Formatting;

public static class StringExtensions
{
    public static IEnumerable<string> SplitLines(this string input)
    {
        return input.Split("\r\n").SelectMany(l => l.Split("\n"));
    }

    public static IEnumerable<(string lineEnding, string line)> SplitLinesPreserveNewLines(this string input)
    {
        var line = new StringBuilder();
        using var enumerator = input.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current is '\r')
            {
                if (enumerator.MoveNext())
                {
                    if (enumerator.Current is '\n')
                    {
                        yield return ("\r\n", line.ToString());
                        line.Clear();
                    }
                    else
                    {
                        line.Append('\r');
                    }
                }
                else
                {
                    yield return ("", line + "\r");

                    yield break;
                }
            }
            else if (enumerator.Current is '\n')
            {
                yield return ("\n", line.ToString());
                line.Clear();
            }
            else
            {
                line.Append(enumerator.Current);
            }
        }

        if (line.Length > 0)
            yield return ("", line.ToString());
    }

    public static string WithoutTrailingWhitespace(this string input)
    {
        var sb = new StringBuilder();
        var consecutiveWhitespace = new List<char>();

        foreach (var character in input)
        {
            switch (character)
            {
                case '\n':
                    sb.Append('\n');
                    consecutiveWhitespace.Clear();
                    continue;
                case ' ' or '\t':
                    consecutiveWhitespace.Add(character);
                    continue;
                default:
                    foreach (var whitespace in consecutiveWhitespace)
                        sb.Append(whitespace);

                    consecutiveWhitespace.Clear();

                    sb.Append(character);
                    break;
            }
        }

        return sb.ToString();
    }
}