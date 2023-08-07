using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UnclosedStringException : TokenizerException
{
    /// <inheritdoc />
    public UnclosedStringException(TokenLocation? location, char stringDelimiter) : base(location, $"Unclosed string. Missing {(stringDelimiter == '"' ? "'\"' (double quote)" : "\"'\" (single quote)")}")
    {
    }
}