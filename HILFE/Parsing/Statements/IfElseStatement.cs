using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class IfElseStatement : BaseStatement
{
    /// <inheritdoc />
    public IfElseStatement(IReadOnlyList<Token> tokens) : base(StatementType.IfElseStatement, tokens)
    {
    }
}