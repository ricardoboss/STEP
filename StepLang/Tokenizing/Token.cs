using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

/// <summary>
/// Represents the smallest unit of a StepLang program.
/// A token has a value, a type and, if available, a location in the source file.
/// </summary>
/// <seealso cref="TokenType"/>
public class Token
{
    /// <summary>
    /// Gets the type of the token.
    /// </summary>
    public TokenType Type { get; }

    /// <summary>
    /// Gets the value of the token.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the location of the token in the source file.
    /// </summary>
    public TokenLocation Location { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class.
    /// </summary>
    /// <param name="type">The type of the token.</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="location">The location of the token in the source file.</param>
    public Token(TokenType type, string value, TokenLocation? location = null)
    {
        Type = type;
        Value = value;
        Location = location ?? new();
    }

    /// <summary>
    /// Gets the content of the token.
    /// If the token is a <see cref="TokenType.LiteralString"/>, the content of the string is returned without the quotes.
    /// </summary>
    public string StringValue => Type switch
    {
        TokenType.LiteralString => Value[1..^1], // cut off the quotes
        _ => Value,
    };

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var printableValue = Value
            .Replace("\n", "\\n", StringComparison.InvariantCulture)
            .Replace("\r", "\\r", StringComparison.InvariantCulture);

        return Value.Length > 0 ? $"<{Type}: '{printableValue}'>" : $"<{Type}>";
    }
}