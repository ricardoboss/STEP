using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class CodeBlockEndStatement : BaseStatement, IExecutableStatement
{
    /// <inheritdoc />
    public CodeBlockEndStatement(IReadOnlyList<Token> tokens) : base(StatementType.CodeBlockEnd, tokens)
    {
    }

    /// <inheritdoc />
    public Task ExecuteAsync(Interpreter interpreter)
    {
        interpreter.PopScope();

        return Task.CompletedTask;
    }
}