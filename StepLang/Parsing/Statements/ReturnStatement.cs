using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Parsing.Statements;

public class ReturnStatement : Statement
{
    private readonly Expression? expression;
    
    public ReturnStatement(Expression? expression) : base(StatementType.ReturnStatement)
    {
        this.expression = expression;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = ExpressionResult.Void;

        if (expression is not null)
            result = await expression.EvaluateAsync(interpreter, cancellationToken);

        interpreter.CurrentScope.SetResult(result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent() => expression?.ToString() ?? ExpressionResult.Void.ToString();
}