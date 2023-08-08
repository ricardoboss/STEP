namespace StepLang.Tokenizing;

public class UnclosedStringException : TokenizerException
{
    private static string BuildMessage() => "A string was not closed.";

    private static string BuildHelpText(char stringDelimiter)
    {
        return $"A string is missing a {(stringDelimiter == '"' ? "'\"' (double quote)" : "\"'\" (single quote)")}. Strings must be closed with a matching delimiter.";
    }

    /// <inheritdoc />
    public UnclosedStringException(TokenLocation? location, char stringDelimiter) : base(BuildMessage(), location, BuildHelpText(stringDelimiter))
    {
    }
}