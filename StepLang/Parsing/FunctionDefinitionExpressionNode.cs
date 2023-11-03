using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public sealed record FunctionDefinitionExpressionNode(IReadOnlyCollection<IVariableDeclarationNode> Arguments, IReadOnlyCollection<StatementNode> Body) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}