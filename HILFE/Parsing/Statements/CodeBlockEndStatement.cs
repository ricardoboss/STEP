using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class CodeBlockEndStatement : BaseStatement
{
    /// <inheritdoc />
    public CodeBlockEndStatement(IReadOnlyList<Token> tokens) : base(StatementType.CodeBlockEnd, tokens)
    {
    }

    /// <inheritdoc />
    public override Task ExecuteAsync(Interpreter interpreter)
    {
        interpreter.Scope.Pop();

        return Task.CompletedTask;
    }
}