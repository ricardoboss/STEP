using System.Text;

namespace StepLang.Formatters;

public static class StringExtensions
{
    public static IEnumerable<string> SplitLines(this string input)
    {
        return input.Split("\r\n").SelectMany(l => l.Split("\n"));
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