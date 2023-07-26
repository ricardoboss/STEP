using System.Diagnostics.CodeAnalysis;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Parsing.Statements;

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

        return result is { ValueType: "bool", Value: true };
    }

    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        if (!await ShouldEnterBranch(interpreter, cancellationToken))
            return;

        interpreter.PushScope();

        await interpreter.InterpretAsync(statements.ToAsyncEnumerable(), cancellationToken);

        interpreter.PopScope();
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"if ({condition}) {{ [{statements.Count} statements] }}";
    }
}