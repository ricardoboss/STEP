using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

public class Token
{
    public TokenType Type { get; }

    public string Value { get; }

    public TokenLocation? Location { get; }

    public FileSystemInfo? File => Location?.File;

    public int? Line => Location?.Line;

    public int? Column => Location?.Column;

    public Token(TokenType type, string value, TokenLocation? location)
    {
        Type = type;
        Value = value;
        Location = location;
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var printableValue = Value
            .Replace("\n", "\\n", StringComparison.InvariantCulture)
            .Replace("\r", "\\r", StringComparison.InvariantCulture);

        return Value.Length > 0 ? $"<{Type}: '{printableValue}'>" : $"<{Type}>";
    }
}