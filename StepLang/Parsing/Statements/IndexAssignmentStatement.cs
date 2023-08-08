using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class IndexAssignmentStatement : Statement
{
    private readonly Token identifier;
    private readonly Expression indexExpression;
    private readonly Expression valueExpression;

    public IndexAssignmentStatement(Token identifier, Expression indexExpression, Expression valueExpression) : base(StatementType.IndexAssignment)
    {
        if (identifier.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifier, TokenType.Identifier);

        this.identifier = identifier;
        this.indexExpression = indexExpression;
        this.valueExpression = valueExpression;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var indexedVariable = interpreter.CurrentScope.GetVariable(identifier);
        var indexResult = await indexExpression.EvaluateAsync(interpreter, cancellationToken);
        var valueResult = await valueExpression.EvaluateAsync(interpreter, cancellationToken);

        switch (indexedVariable.Value.ValueType)
        {
            case "list":
                var list = indexedVariable.Value.ExpectList();
                var index = indexResult.ExpectIntegerIndex(list.Count);

                list[index] = valueResult;
                break;
            case "map":
                var map = indexedVariable.Value.ExpectMap();
                var key = indexResult.ExpectString();

                map[key] = valueResult;
                break;
            default:
                throw new InvalidIndexOperatorException(identifier.Location, indexResult.Value?.ToString() ?? "<null>", indexedVariable.Value.ValueType, "assign");
        }
    }

    /// <inheritdoc />
    protected override string DebugRenderContent() => $"{identifier.Value}[{indexExpression}] = {valueExpression}";
}