namespace StepLang.Tokenizing;

public class UnclosedStringException : TokenizerException
{
    /// <inheritdoc />
    public UnclosedStringException(TokenLocation? location, char stringDelimiter) : base(location, $"A string is missing a {(stringDelimiter == '"' ? "'\"' (double quote)" : "\"'\" (single quote)")}")
    {
    }
}