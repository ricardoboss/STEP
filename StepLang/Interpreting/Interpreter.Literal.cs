using StepLang.Expressions.Results;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public ExpressionResult Evaluate(LiteralExpressionNode expressionNode)
    {
        var literal = expressionNode.Literal;

        return literal.Type switch
        {
            TokenType.LiteralBoolean => (BoolResult)literal.Value,
            TokenType.LiteralNumber => (NumberResult)literal.Value,
            TokenType.LiteralString => (StringResult)literal.StringValue,
            TokenType.LiteralNull => NullResult.Instance,
            _ => throw new NotImplementedException("Unimplemented literal type " + literal),
        };
    }

    public ExpressionResult Evaluate(ListExpressionNode expressionNode)
    {
        var results = expressionNode
            .Expressions
            .Select(expression => expression.EvaluateUsing(this))
            .ToList();

        return new ListResult(results);
    }

    public ExpressionResult Evaluate(MapExpressionNode expressionNode)
    {
        var results = expressionNode
            .Expressions
            .ToDictionary(
                kvp => kvp.Key.StringValue,
                kvp => kvp.Value.EvaluateUsing(this)
            );

        return new MapResult(results);
    }
}