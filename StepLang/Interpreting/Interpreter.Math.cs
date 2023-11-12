using StepLang.Expressions.Results;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    private (ExpressionResult left, ExpressionResult right, TokenLocation location) EvaluateBinary(IBinaryExpressionNode expressionNode)
    {
        var leftResult = expressionNode.Left.EvaluateUsing(this);
        var rightResult = expressionNode.Right.EvaluateUsing(this);
        var location = expressionNode.OperatorLocation;

        return (leftResult, rightResult, location);
    }

    public ExpressionResult Evaluate(AddExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber + bNumber,
            NumberResult aNumber when right is StringResult bString => aNumber + bString,
            StringResult aString when right is NumberResult bNumber => aString + bNumber,
            StringResult aString when right is StringResult bString => aString + bString,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "add"),
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
        var (left, right, location) = EvaluateBinary(expressionNode);

        if (left is VoidResult || right is VoidResult)
            throw new IncompatibleExpressionOperandsException(location, left, right, "compare (not equals)");

        if (left is NullResult && right is NullResult)
            return BoolResult.False;

        if (left is NullResult || right is NullResult)
            return BoolResult.True;

        return left switch
        {
            StringResult aString when right is StringResult bString => aString != bString,
            NumberResult aNumber when right is NumberResult bNumber => aNumber != bNumber,
            BoolResult aBool when right is BoolResult bBool => aBool != bBool,
            _ => BoolResult.True,
        };
    }

    public ExpressionResult Evaluate(EqualsExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        if (left is VoidResult || right is VoidResult)
            throw new IncompatibleExpressionOperandsException(location, left, right, "compare (equals)");

        if (left is NullResult && right is NullResult)
            return BoolResult.True;

        if (left is NullResult || right is NullResult)
            return BoolResult.False;

        return left switch
        {
            StringResult aString when right is StringResult bString => aString == bString,
            NumberResult aNumber when right is NumberResult bNumber => aNumber == bNumber,
            BoolResult aBool when right is BoolResult bBool => aBool == bBool,
            _ => BoolResult.False,
        };
    }

    public ExpressionResult Evaluate(NegateExpressionNode expressionNode)
    {
        var result = expressionNode.Expression.EvaluateUsing(this);

        return result switch
        {
            NumberResult number => -number,
            _ => throw new IncompatibleExpressionOperandsException(expressionNode.Location, result, "negate"),
        };
    }

    public ExpressionResult Evaluate(SubtractExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber - bNumber,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "subtract"),
        };
    }

    public ExpressionResult Evaluate(MultiplyExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber * bNumber,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "multiply"),
        };
    }

    public ExpressionResult Evaluate(DivideExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        // TODO: throw a StepLangException when dividing by zero to provide more useful info

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber / bNumber,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "divide"),
        };
    }

    public ExpressionResult Evaluate(ModuloExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber % bNumber,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "modulo"),
        };
    }

    public ExpressionResult Evaluate(PowerExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        if (left is not NumberResult baseNumber || right is not NumberResult exponentNumber)
            throw new IncompatibleExpressionOperandsException(location, left, right, "power");

        return (NumberResult)Math.Pow(baseNumber, exponentNumber);
    }

    public ExpressionResult Evaluate(GreaterThanExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber > bNumber,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "compare (greater than)"),
        };
    }

    public ExpressionResult Evaluate(LessThanExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber < bNumber,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "compare (less than)"),
        };
    }

    public ExpressionResult Evaluate(GreaterThanOrEqualExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber >= bNumber,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "compare (greater than or equal to)"),
        };
    }

    public ExpressionResult Evaluate(LessThanOrEqualExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            NumberResult aNumber when right is NumberResult bNumber => aNumber <= bNumber,
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "compare (less than or equal to)"),
        };
    }

    public ExpressionResult Evaluate(LogicalAndExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            BoolResult aBool when right is BoolResult bBool => (BoolResult)(aBool && bBool),
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "logical and"),
        };
    }

    public ExpressionResult Evaluate(LogicalOrExpressionNode expressionNode)
    {
        var (left, right, location) = EvaluateBinary(expressionNode);

        return left switch
        {
            BoolResult aBool when right is BoolResult bBool => (BoolResult)(aBool || bBool),
            _ => throw new IncompatibleExpressionOperandsException(location, left, right, "logical or"),
        };
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
        var result = expressionNode.Expression.EvaluateUsing(this);

        return result switch
        {
            BoolResult boolResult => !boolResult,
            _ => throw new IncompatibleExpressionOperandsException(expressionNode.Location, result, "not"),
        };
    }
}
