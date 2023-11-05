using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record MapExpressionNode(Token OpenCurlyBraceToken, IReadOnlyDictionary<Token, ExpressionNode> Expressions) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }

    public override TokenLocation Location => OpenCurlyBraceToken.Location;
}