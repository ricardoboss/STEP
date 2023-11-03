using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public abstract record ExpressionNode : IEvaluatableNode<IExpressionEvaluator, ExpressionResult>
{
    public abstract ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator);
}