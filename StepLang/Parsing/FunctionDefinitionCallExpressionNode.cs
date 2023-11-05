using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record FunctionDefinitionCallExpressionNode(Token OpenParenthesisToken, IReadOnlyList<IVariableDeclarationNode> Parameters, IReadOnlyList<StatementNode> Body, IReadOnlyList<ExpressionNode> CallArguments) : ExpressionNode
{
    public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
    {
        return evaluator.Evaluate(this);
    }

    public override TokenLocation Location => OpenParenthesisToken.Location;
}