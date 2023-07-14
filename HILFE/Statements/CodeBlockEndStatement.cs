namespace HILFE.Statements;

public class CodeBlockEndStatement : BaseStatement
{
    /// <inheritdoc />
    public CodeBlockEndStatement(IReadOnlyList<Token> tokens) : base(StatementType.CodeBlockStart, tokens)
    {
    }

    /// <inheritdoc />
    public override Task ExecuteAsync(Interpreter interpreter)
    {
        interpreter.Scope.Pop();

        return Task.CompletedTask;
    }
}