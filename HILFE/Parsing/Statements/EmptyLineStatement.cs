using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class EmptyLineStatement : BaseStatement
{
    /// <inheritdoc />
    public EmptyLineStatement(IReadOnlyList<Token> tokens) : base(StatementType.EmptyLine, tokens)
    {
    }
}