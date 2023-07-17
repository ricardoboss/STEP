using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public abstract class BaseStatement
{
    public BaseStatement(StatementType type, IReadOnlyList<Token> tokens)
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
