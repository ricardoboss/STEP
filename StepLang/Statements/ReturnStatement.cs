using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Statements;

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
        ExpressionResult result = VoidResult.Instance;

        if (expression is not null)
            result = await expression.EvaluateAsync(interpreter, cancellationToken);

        interpreter.CurrentScope.SetResult(result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent() => expression?.ToString() ?? VoidResult.Instance.ToString();
}