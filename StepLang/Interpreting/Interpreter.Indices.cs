using System.Globalization;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(IdentifierIndexAssignmentNode statementNode)
    {
        var variable = CurrentScope.GetVariable(statementNode.Identifier);
        var indexResult = statementNode.IndexExpression.EvaluateUsing(this);
        var valueResult = statementNode.ValueExpression.EvaluateUsing(this);

        switch (variable.Value)
        {
            case ListResult { Value: var list } when indexResult is NumberResult index:
                list[index] = valueResult;
                break;
            case MapResult { Value: var map } when indexResult is StringResult { Value: var key }:
                map[key] = valueResult;
                break;
            default:
                var indexRepresentation = indexResult switch
                {
                    StringResult stringResult => stringResult.Value,
                    NumberResult numberResult => numberResult.Value.ToString(CultureInfo.InvariantCulture),
                    _ => indexResult.ToString(),
                };

                throw new InvalidIndexOperatorException(null, indexRepresentation, variable.Value.ResultType, "assign");
        }
    }

    public ExpressionResult Evaluate(IdentifierIndexAccessExpressionNode expressionNode)
    {
        var variable = CurrentScope.GetVariable(expressionNode.Identifier);
        var indexResult = expressionNode.Index.EvaluateUsing(this);

        switch (variable.Value)
        {
            case ListResult { Value: var values } when indexResult is NumberResult index:
            {
                if (index < 0 || index >= values.Count)
                    throw new IndexOutOfBoundsException(index, values.Count);

                return values[index];
            }
            case MapResult { Value: var pairs } when indexResult is StringResult { Value: var key }:
                return pairs[key];
            case StringResult { Value: var str } when indexResult is NumberResult index:
            {
                var grapheme = str.GraphemeAt(index);
                if (grapheme == null)
                    throw new IndexOutOfBoundsException(index, str.GraphemeLength());

                return new StringResult(grapheme);
            }
            default:
            {
                var indexRepresentation = indexResult switch
                {
                    StringResult stringResult => stringResult.Value,
                    NumberResult numberResult => numberResult.Value.ToString(CultureInfo.InvariantCulture),
                    _ => indexResult.ToString(),
                };

                throw new InvalidIndexOperatorException(expressionNode.Identifier.Location, indexRepresentation, variable.Value.ResultType, "access");
            }
        }
    }
}