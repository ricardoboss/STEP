using System.Diagnostics.CodeAnalysis;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Parsing.Statements;

public class ReturnStatement : Statement
{
    private readonly Expression expression;
    
    public ReturnStatement(Expression expression) : base(StatementType.ReturnStatement)
    {
        this.expression = expression;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = await expression.EvaluateAsync(interpreter, cancellationToken);
        if (result is null or { IsVoid: true })
            throw new InterpreterException("Cannot assign a void value to a variable");

        interpreter.CurrentScope.SetResult(result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent() => expression.ToString();
}