using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public record FunctionDefinitionCallExpressionNode(IReadOnlyList<IVariableDeclarationNode> Parameters, IReadOnlyList<StatementNode> Body, IReadOnlyList<ExpressionNode> CallArguments) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }
}