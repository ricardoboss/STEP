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

        var invalidChar = value.GetFirstInvalidIdentifierCharacter();

        return generalHelp + $"Yours contains an invalid character: '{invalidChar!.Value}'";
    }

    public InvalidIdentifierException(TokenLocation? location, string value) : base(BuildMessage(value), location, BuildHelpText(value))
    {
    }
}