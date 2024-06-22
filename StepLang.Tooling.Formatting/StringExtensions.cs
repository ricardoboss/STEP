using System.Text;

namespace StepLang.Tooling.Formatting;

/// <summary>
/// A collection of extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Splits a string by line endings, no matter what the line ending is (<c>\r\n</c> or <c>\n</c>).
    /// </summary>
    /// <param name="input">The string to split.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of lines.</returns>
    public static IEnumerable<string> SplitLines(this string input)
    {
        return input.Split("\r\n").SelectMany(l => l.Split("\n"));
    }

    /// <summary>
    /// Splits a string by line endings, no matter what the line ending is (<c>\r\n</c> or <c>\n</c>) and returns the line ending with the line.
    /// </summary>
    /// <param name="input">The string to split.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of lines and their line endings.</returns>
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

    /// <summary>
    /// Removes all trailing whitespace from a string while preserving line endings.
    /// </summary>
    /// <param name="input">The string to remove trailing whitespace from.</param>
    /// <returns>The string without trailing whitespace.</returns>
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

    /// <summary>
    /// Goes through the string and calls the callback for each word (i.e. a sequence of characters surrounded by whitespace or line endings).
    /// </summary>
    /// <param name="input">The string to go through.</param>
    /// <param name="callback">The callback to call for each word.</param>
    /// <returns>The string with the callback applied to each word.</returns>
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