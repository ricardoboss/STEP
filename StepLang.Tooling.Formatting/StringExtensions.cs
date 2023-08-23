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

    public static string SelectWords(this IEnumerable<char> input, Func<string, string> callback)
    {

        StringBuilder output = new();
        StringBuilder currentWord = new();
        var inString = false;
        var escaped = false;

        using var enumerator = input.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var currentChar = enumerator.Current;
            switch (currentChar)
            {
                case '"' when !escaped:
                    inString = !inString;
                    output.Append(currentChar);
                    continue;
                case '\\' when inString:
                    escaped = true;
                    output.Append(currentChar);
                    continue;
            }

            if (inString)
            {
                output.Append(currentChar);
                continue;
            }

            if (char.IsLetterOrDigit(currentChar))
            {
                currentWord.Append(currentChar);
                continue;
            }

            if (currentWord.Length > 0)
                SelectWord();

            output.Append(currentChar);
        }

        if (currentWord.Length > 0 && !inString)
            SelectWord();

        return output.ToString();

        void SelectWord()
        {
            var word = currentWord.ToString();

            output.Append(callback(word));

            currentWord.Clear();
        }
    }
}