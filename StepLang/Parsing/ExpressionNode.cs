using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public abstract record ExpressionNode : IEvaluatableNode<IExpressionEvaluator, ExpressionResult>
{
    public abstract ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator);

    public abstract TokenLocation Location { get; }
}