namespace HILFE.Tokenizing;

public class Token
{
    public readonly TokenType Type;
    public readonly string Value;

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return Value.Length > 0 ? $"<{Type}: '{Value}'>" : $"<{Type}>";
    }
}