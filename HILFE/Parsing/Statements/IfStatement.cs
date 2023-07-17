using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class IfStatement : BaseStatement, IBranchingStatement
{
    /// <inheritdoc />
    public IfStatement(IReadOnlyList<Token> tokens) : base(StatementType.IfStatement, tokens)
    {
    }

    public Task<bool> ShouldBranch(Interpreter interpreter)
    {
        return Task.FromResult(false);
    }
}