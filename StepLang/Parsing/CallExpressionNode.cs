using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record CallExpressionNode(Token Identifier, IReadOnlyList<ExpressionNode> Arguments) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }

    public override TokenLocation Location => Identifier.Location;
}