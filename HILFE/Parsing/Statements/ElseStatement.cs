using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class ElseStatement : BaseStatement
{
    /// <inheritdoc />
    public ElseStatement(IReadOnlyList<Token> tokens) : base(StatementType.ElseStatement, tokens)
    {
    }
}