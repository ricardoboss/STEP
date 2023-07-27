using HILFE.Interpreting;
using HILFE.Parsing.Expressions;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class IndexAssignmentStatement : Statement
{
    private readonly Token identifier;
    private readonly Expression indexExpression;
    private readonly Expression valueExpression;

    public IndexAssignmentStatement(Token identifier, Expression indexExpression, Expression valueExpression) : base(StatementType.IndexAssignment)
    {
        if (identifier.Type != TokenType.Identifier)
            throw new TokenizerException($"Expected {TokenType.Identifier} token, but got {identifier.Type} instead");

        this.identifier = identifier;
        this.indexExpression = indexExpression;
        this.valueExpression = valueExpression;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var listVariable = interpreter.CurrentScope.GetVariable(identifier.Value);
        var list = listVariable.Value.ExpectList();

        var indexResult = await indexExpression.EvaluateAsync(interpreter, cancellationToken);
        var index = indexResult.ExpectListIndex(list.Count);

        var valueResult = await valueExpression.EvaluateAsync(interpreter, cancellationToken);

        list[index] = valueResult;
    }

    /// <inheritdoc />
    protected override string DebugRenderContent() => $"{identifier.Value}[{indexExpression}] = {valueExpression}";
}