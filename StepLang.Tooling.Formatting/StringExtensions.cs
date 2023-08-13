using System.Text;

namespace StepLang.Tooling.Formatting;

public static class StringExtensions
{
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