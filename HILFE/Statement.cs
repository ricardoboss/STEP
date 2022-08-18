namespace HILFE;

public class Statement
{
    public Statement(StatementType type, IReadOnlyList<Token> tokens)
    {
        Type = type;
        Tokens = tokens;
    }

    public readonly StatementType Type;

    public readonly IReadOnlyList<Token> Tokens;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[{Type}: {string.Join(", ", Tokens)}]";
    }
}
