using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Statements;

public class IfStatement : Statement
{
    private readonly Expression condition;
    private readonly IReadOnlyList<Statement> statements;

    /// <inheritdoc />
    public IfStatement(Expression condition, IReadOnlyList<Statement> statements) : base(StatementType.IfStatement)
    {
        this.condition = condition;
        this.statements = statements;
    }

    private async Task<bool> ShouldEnterBranch(Interpreter interpreter, CancellationToken cancellationToken)
    {
        var result = await condition.EvaluateAsync(interpreter, cancellationToken);

        return result is BoolResult { Value: true };
    }

    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        interpreter.PushScope();

        if (await ShouldEnterBranch(interpreter, cancellationToken))
            await interpreter.InterpretAsync(statements.ToAsyncEnumerable(), cancellationToken);

        var previousScope = interpreter.PopScope();
        if (previousScope.TryGetResult(out var result))
            interpreter.CurrentScope.SetResult(result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"if ({condition}) {{ [{statements.Count} statements] }}";
    }
}