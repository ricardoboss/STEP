using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public abstract class TokenizerException : Exception
{
    public TokenLocation? Location { get; }

    protected TokenizerException(TokenLocation? location, string message) : base($"{location?.ToString() ?? "<unknown>"}: {message}")
    {
        Location = location;
    }
}