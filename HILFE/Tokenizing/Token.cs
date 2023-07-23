namespace HILFE.Tokenizing;

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        var printableValue = Value
            .Replace("\n", "\\n", StringComparison.InvariantCulture)
            .Replace("\r", "\\r", StringComparison.InvariantCulture);

        return Value.Length > 0 ? $"<{Type}: '{printableValue}'>" : $"<{Type}>";
    }
}