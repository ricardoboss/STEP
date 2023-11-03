using StepLang.Expressions.Results;

namespace StepLang.Parsing;

public interface IExpressionEvaluator
{
    ExpressionResult Evaluate(CallExpressionNode expressionNode);
    ExpressionResult Evaluate(FunctionDefinitionExpressionNode expressionNode);
    ExpressionResult Evaluate(IdentifierExpressionNode expressionNode);
    ExpressionResult Evaluate(IdentifierIndexAccessExpressionNode expressionNode);
    ExpressionResult Evaluate(ListExpressionNode expressionNode);
    ExpressionResult Evaluate(LiteralExpressionNode expressionNode);
    ExpressionResult Evaluate(MapExpressionNode expressionNode);
    ExpressionResult Evaluate(PrimaryExpressionNode expressionNode);
    ExpressionResult Evaluate(AddExpressionNode expressionNode);
    ExpressionResult Evaluate(CoalesceExpressionNode expressionNode);
    ExpressionResult Evaluate(NotEqualsExpressionNode expressionNode);
    ExpressionResult Evaluate(EqualsExpressionNode expressionNode);
    ExpressionResult Evaluate(NegateExpressionNode expressionNode);
}