using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class IfElseStatement : BaseStatement
{
    /// <inheritdoc />
    public IfElseStatement(IReadOnlyList<Token> tokens) : base(StatementType.IfStatement, tokens)
    {
    }

    /// <inheritdoc />
    public override Task ExecuteAsync(Interpreter interpreter)
    {
        return Task.CompletedTask;
    }
}