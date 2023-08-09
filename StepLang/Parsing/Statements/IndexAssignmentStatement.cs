using System.Globalization;
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

        switch (indexedVariable.Value)
        {
            case ListResult { Value: var list }:
                var index = (int)indexResult.ExpectInteger().Value;

                list[index] = valueResult;
                break;
            case MapResult { Value: var map }:
                var key = indexResult.ExpectString().Value;

                map[key] = valueResult;
                break;
            default:
                var indexRepresentation = indexResult switch
                {
                    StringResult stringResult => stringResult.Value,
                    NumberResult numberResult => numberResult.Value.ToString(CultureInfo.InvariantCulture),
                    _ => indexResult.ToString(),
                };

                throw new InvalidIndexOperatorException(null, indexRepresentation, indexedVariable.Value.ResultType, "assign");
        }
    }

    /// <inheritdoc />
    protected override string DebugRenderContent() => $"{identifier.Value}[{indexExpression}] = {valueExpression}";
}