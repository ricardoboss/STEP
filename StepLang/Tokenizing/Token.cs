using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

public class Token
{
    public TokenType Type { get; }

    public string Value { get; }

    public TokenLocation Location { get; }

    public static Token FromEndLocation(TokenType type, string value, TokenLocation endLocation)
    {
        var startColumn = endLocation.Column - value.Length;

        var startLocation = endLocation with { Column = startColumn };

        return new(type, value, startLocation);
    }

    public Token(TokenType type, string value, TokenLocation? location = null)
    {
        Type = type;
        Value = value;
        Location = location ?? new();
    }

    public string StringValue => Type switch
    {
        TokenType.LiteralString => Value[1..^1], // cut off the quotes
        _ => Value,
    };

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var printableValue = Value
            .Replace("\n", "\\n", StringComparison.InvariantCulture)
            .Replace("\r", "\\r", StringComparison.InvariantCulture);

        return Value.Length > 0 ? $"<{Type}: '{printableValue}'>" : $"<{Type}>";
    }
}