using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Statements;

public class WhileStatement : Statement
{
    private readonly Expression condition;
    private readonly IReadOnlyList<Statement> statements;

    /// <inheritdoc />
    public WhileStatement(Expression condition, IReadOnlyList<Statement> statements) : base(StatementType.WhileStatement)
    {
        this.condition = condition;
        this.statements = statements;
    }

    public async Task<bool> ShouldLoopAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = await condition.EvaluateAsync(interpreter, cancellationToken);

        return result is BoolResult { Value: true };
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        interpreter.PushScope();

        while (await ShouldLoopAsync(interpreter, cancellationToken))
        {
            await interpreter.InterpretAsync(statements.ToAsyncEnumerable(), cancellationToken);

            if (interpreter.BreakDepth <= 0)
                continue;

            interpreter.BreakDepth--;

            break;
        }

        interpreter.PopScope();
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"{condition} {{ [{statements.Count} statements] }}";
    }
}