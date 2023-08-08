using System.Diagnostics;

namespace StepLang.Tokenizing;

internal sealed class InvalidIdentifierException : TokenizerException
{
    private static string BuildMessage(string value)
    {
        return $"Invalid identifier '{value}'";
    }

    private static string BuildHelpText(string value)
    {
        const string generalHelp = "Identifiers must start with a letter and can only contain letters, numbers, and underscores. ";

        if (value.Trim().Length == 0)
            return generalHelp + "Yours is empty.";

        if (char.IsDigit(value[0]))
            return generalHelp + "Yours starts with a number.";

        var invalidChar = GetFirstInvalidIdentifierCharacter(value);

        Debug.Assert(invalidChar.HasValue);

        return generalHelp + $"Yours contains an invalid character: '{invalidChar.Value}'";
    }

    private static char? GetFirstInvalidIdentifierCharacter(string value)
    {
        if (value.Trim().Length == 0)
            return null;

        if (value.Contains('.', StringComparison.InvariantCulture))
            return '.';

        var firstChar = value[0];
        if (firstChar is (< 'a' or > 'z') and (< 'A' or > 'Z') and not '_')
            return firstChar;

        for (var i = 1; i < value.Length; i++)
        {
            var c = value[i];
            if (c is not (>= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9' or '_'))
                return c;
        }

        return null;
    }

    public InvalidIdentifierException(TokenLocation? location, string value) : base(BuildMessage(value), location, BuildHelpText(value))
    {
    }
}