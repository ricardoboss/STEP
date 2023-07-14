using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class CodeBlockStartStatement : BaseStatement
{
    /// <inheritdoc />
    public CodeBlockStartStatement(IReadOnlyList<Token> tokens) : base(StatementType.CodeBlockStart, tokens)
    {
    }

    /// <inheritdoc />
    public override Task ExecuteAsync(Interpreter interpreter)
    {
        interpreter.Scope.Push();

        return Task.CompletedTask;
    }
}