using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    private (ExpressionResult left, ExpressionResult right) VisitBinaryExpression(IBinaryExpressionNode expressionNode)
    {
        var leftResult = expressionNode.Left.EvaluateUsing(this);
        var rightResult = expressionNode.Right.EvaluateUsing(this);

        return (leftResult, rightResult);
    }

    public ExpressionResult Evaluate(AddExpressionNode expressionNode)
    {
        var (left, right) = VisitBinaryExpression(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => new NumberResult(aNumber.Value + bNumber.Value),
            NumberResult aNumber when right is StringResult bString => new StringResult(aNumber.Value + bString.Value),
            StringResult aString when right is NumberResult bNumber => new StringResult(aString.Value + bNumber.Value),
            StringResult aString when right is StringResult bString => new StringResult(aString.Value + bString.Value),
            _ => throw new IncompatibleExpressionOperandsException(left, right, "add"),
        };
    }

    public ExpressionResult Evaluate(CoalesceExpressionNode expressionNode)
    {
        var leftResult = expressionNode.Left.EvaluateUsing(this);
        if (leftResult is not NullResult)
            return leftResult;

        return expressionNode.Right.EvaluateUsing(this);
    }

    public ExpressionResult Evaluate(NotEqualsExpressionNode expressionNode)
    {
        var (left, right) = VisitBinaryExpression(expressionNode);

        if (left is VoidResult || right is VoidResult)
            throw new IncompatibleExpressionOperandsException(left, right, "compare (not equals)");

        if (left is NullResult && right is NullResult)
            return new BoolResult(false);

        if (left is NullResult || right is NullResult)
            return new BoolResult(true);

        return new BoolResult(left switch
        {
            StringResult aString when right is StringResult bString => !string.Equals(aString.Value, bString.Value, StringComparison.Ordinal),
            NumberResult aNumber when right is NumberResult bNumber => Math.Abs(aNumber.Value - bNumber.Value) >= double.Epsilon,
            BoolResult aBool when right is BoolResult bBool => aBool.Value != bBool.Value,
            _ => true,
        });
    }

    public ExpressionResult Evaluate(EqualsExpressionNode expressionNode)
    {
        var (left, right) = VisitBinaryExpression(expressionNode);

        if (left is VoidResult || right is VoidResult)
            throw new IncompatibleExpressionOperandsException(left, right, "compare (equals)");

        if (left is NullResult && right is NullResult)
            return new BoolResult(true);

        if (left is NullResult || right is NullResult)
            return new BoolResult(false);

        return new BoolResult(left switch
        {
            StringResult aString when right is StringResult bString => string.Equals(aString.Value, bString.Value, StringComparison.Ordinal),
            NumberResult aNumber when right is NumberResult bNumber => Math.Abs(aNumber.Value - bNumber.Value) < double.Epsilon,
            BoolResult aBool when right is BoolResult bBool => aBool.Value == bBool.Value,
            _ => false,
        });
    }

    public ExpressionResult Evaluate(NegateExpressionNode expressionNode)
    {
        var result = expressionNode.Expression.EvaluateUsing(this);

        return result switch
        {
            NumberResult number => new NumberResult(-number.Value),
            _ => throw new IncompatibleExpressionOperandsException(result, "negate"),
        };
    }

    public ExpressionResult Evaluate(SubtractExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(MultiplyExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(DivideExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(ModuloExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(PowerExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(GreaterThanExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(LessThanExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(GreaterThanOrEqualExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(LessThanOrEqualExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(LogicalAndExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(LogicalOrExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(BitwiseXorExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(BitwiseAndExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(BitwiseOrExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(BitwiseShiftLeftExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(BitwiseShiftRightExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(BitwiseRotateLeftExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(BitwiseRotateRightExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public ExpressionResult Evaluate(NotExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }

    public void Visit(IncrementStatementNode incrementStatementNode)
    {
        throw new NotImplementedException();
    }

    public void Visit(DecrementStatementNode incrementStatementNode)
    {
        throw new NotImplementedException();
    }
}
