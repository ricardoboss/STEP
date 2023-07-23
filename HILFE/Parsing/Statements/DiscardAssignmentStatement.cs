using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Parsing.Statements;

public class DiscardAssignmentStatement : Statement
{
    private readonly Expression expression;

    public DiscardAssignmentStatement(Expression expression) : base(StatementType.DiscardAssignment)
    {
        this.expression = expression;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        _ = await expression.EvaluateAsync(interpreter, cancellationToken);
    }
}