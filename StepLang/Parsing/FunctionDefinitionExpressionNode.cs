using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public sealed record FunctionDefinitionExpressionNode(IReadOnlyList<IVariableDeclarationNode> Parameters, IReadOnlyList<StatementNode> Body) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}