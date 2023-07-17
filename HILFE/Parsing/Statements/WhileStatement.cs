using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class WhileStatement : BaseStatement, ILoopingStatement
{
    /// <inheritdoc />
    public WhileStatement(IReadOnlyList<Token> tokens) : base(StatementType.IfStatement, tokens)
    {
    }

    public Task InitializeLoop(Interpreter interpreter)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ShouldLoop(Interpreter interpreter)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExecuteLoop(IReadOnlyList<BaseStatement> statements)
    {
        throw new NotImplementedException();
    }
}