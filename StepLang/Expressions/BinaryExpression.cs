using System.Globalization;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions;

public class BinaryExpression : Expression
{
    public static BinaryExpression FromOperator(BinaryExpressionOperator op, Expression left, Expression right)
    {
        return op switch
        {
            BinaryExpressionOperator.Power => Power(left, right),
            BinaryExpressionOperator.Plus => Add(left, right),
            BinaryExpressionOperator.Minus => Subtract(left, right),
            BinaryExpressionOperator.Multiply => Multiply(left, right),
            BinaryExpressionOperator.Divide => Divide(left, right),
            BinaryExpressionOperator.Modulo => Modulo(left, right),
            BinaryExpressionOperator.GreaterThan => GreaterThan(left, right),
            BinaryExpressionOperator.GreaterThanOrEqual => GreaterThanOrEqual(left, right),
            BinaryExpressionOperator.LessThan => LessThan(left, right),
            BinaryExpressionOperator.LessThanOrEqual => LessThanOrEqual(left, right),
            BinaryExpressionOperator.Equal => Equals(left, right),
            BinaryExpressionOperator.NotEqual => NotEquals(left, right),
            BinaryExpressionOperator.LogicalAnd => LogicalAnd(left, right),
            BinaryExpressionOperator.LogicalOr => LogicalOr(left, right),
            BinaryExpressionOperator.Coalesce => Coalesce(left, right),
            BinaryExpressionOperator.Index => Index(left, right),
            _ => throw new NotImplementedException($"The operator {op} is not implemented yet"),
        };
    }

    public static BinaryExpression Power(Expression left, Expression right)
    {
        return new BinaryExpression("^", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "power");

            return new NumberResult(Math.Pow(aNumber.Value, bNumber.Value));
        });
    }

    public static BinaryExpression Add(Expression left, Expression right)
    {
        return new BinaryExpression("+", left, right, (a, b) =>
        {
            return a switch
            {
                NumberResult aNumber when b is NumberResult bNumber => new NumberResult(aNumber.Value + bNumber.Value),
                NumberResult aNumber when b is StringResult bString => new StringResult(aNumber.Value + bString.Value),
                StringResult aString when b is NumberResult bNumber => new StringResult(aString.Value + bNumber.Value),
                StringResult aString when b is StringResult bString => new StringResult(aString.Value + bString.Value),
                _ => throw new IncompatibleExpressionOperandsException(a, b, "add"),
            };
        });
    }

    public static BinaryExpression Subtract(Expression left, Expression right)
    {
        return new BinaryExpression("-", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "subtract");

            return new NumberResult(aNumber.Value - bNumber.Value);
        });
    }

    public static BinaryExpression Multiply(Expression left, Expression right)
    {
        return new BinaryExpression("*", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "multiply");

            return new NumberResult(aNumber.Value * bNumber.Value);
        });
    }

    public static BinaryExpression Divide(Expression left, Expression right)
    {
        return new BinaryExpression("/", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "divide");

            // TODO: throw a specific exception when dividing by zero

            return new NumberResult(aNumber.Value / bNumber.Value);
        });
    }

    public static BinaryExpression Modulo(Expression left, Expression right)
    {
        return new BinaryExpression("%", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "modulo");

            return new NumberResult(aNumber.Value % bNumber.Value);
        });
    }

    public static BinaryExpression LessThan(Expression left, Expression right)
    {
        return new BinaryExpression("<", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (less than)");

            return new BoolResult(aNumber.Value < bNumber.Value);
        });
    }

    public static BinaryExpression LessThanOrEqual(Expression left, Expression right)
    {
        return new BinaryExpression("<=", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (less than or equal)");

            return new BoolResult(aNumber.Value <= bNumber.Value);
        });
    }

    public static BinaryExpression GreaterThan(Expression left, Expression right)
    {
        return new BinaryExpression(">", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (greater than)");

            return new BoolResult(aNumber.Value > bNumber.Value);
        });
    }

    public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right)
    {
        return new BinaryExpression(">=", left, right, (a, b) =>
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (greater than or equal)");

            return new BoolResult(aNumber.Value >= bNumber.Value);
        });
    }

    public static BinaryExpression Equals(Expression left, Expression right)
    {
        return new BinaryExpression("==", left, right, (a, b) =>
        {
            if (a is VoidResult || b is VoidResult)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (equals)");

            if (a is NullResult && b is NullResult)
                return new BoolResult(true);

            if (a is NullResult || b is NullResult)
                return new BoolResult(false);

            return new BoolResult(a switch
            {
                StringResult aString when b is StringResult bString => string.Equals(aString.Value, bString.Value, StringComparison.Ordinal),
                NumberResult aNumber when b is NumberResult bNumber => Math.Abs(aNumber.Value - bNumber.Value) < double.Epsilon,
                BoolResult aBool when b is BoolResult bBool => aBool.Value == bBool.Value,
                _ => false,
            });
        });
    }

    public static BinaryExpression NotEquals(Expression left, Expression right)
    {
        return new BinaryExpression("!=", left, right, (a, b) =>
        {
            if (a is VoidResult || b is VoidResult)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (not equals)");

            if (a is NullResult && b is NullResult)
                return new BoolResult(false);

            if (a is NullResult || b is NullResult)
                return new BoolResult(true);

            return new BoolResult(a switch
            {
                StringResult aString when b is StringResult bString => !string.Equals(aString.Value, bString.Value, StringComparison.Ordinal),
                NumberResult aNumber when b is NumberResult bNumber => Math.Abs(aNumber.Value - bNumber.Value) >= double.Epsilon,
                BoolResult aBool when b is BoolResult bBool => aBool.Value != bBool.Value,
                _ => true,
            });
        });
    }

    public static BinaryExpression LogicalOr(Expression left, Expression right)
    {
        return new BinaryExpression("||", left, right, (a, b) =>
        {
            if (a is not BoolResult aBool || b is not BoolResult bBool)
                throw new IncompatibleExpressionOperandsException(a, b, "logical or");

            return new BoolResult(aBool.Value || bBool.Value);
        });
    }

    public static BinaryExpression LogicalAnd(Expression left, Expression right)
    {
        return new BinaryExpression("&&", left, right, (a, b) =>
        {
            if (a is not BoolResult aBool || b is not BoolResult bBool)
                throw new IncompatibleExpressionOperandsException(a, b, "logical and");

            return new BoolResult(aBool.Value && bBool.Value);
        });
    }

    public static BinaryExpression Coalesce(Expression left, Expression right)
    {
        return new BinaryExpression("??", left, right, (a, b) => a is NullResult ? b : a);
    }

    public static BinaryExpression Index(Expression left, Expression right)
    {
        return new BinaryExpression("[]", left, right, (a, b) =>
        {
            switch (a.ResultType)
            {
                case ResultType.List:
                    {
                        var values = a.ExpectList().Value;
                        var index = b.ExpectInteger().RoundedIntValue;

                        if (index < 0 || index >= values.Count)
                            throw new IndexOutOfBoundsException(index, values.Count);

                        return values[index];
                    }
                case ResultType.Map:
                    {
                        var pairs = a.ExpectMap().Value;
                        var key = b.ExpectString().Value;

                        return pairs[key];
                    }
                case ResultType.Str:
                    {
                        var value = a.ExpectString().Value;
                        var index = b.ExpectInteger().RoundedIntValue;

                        if (index < 0 || index >= value.Length)
                            throw new IndexOutOfBoundsException(index, value.Length);

                        return new StringResult(value[index].ToString());
                    }
                default:
                    var indexRepresentation = b switch
                    {
                        StringResult stringResult => stringResult.Value,
                        NumberResult numberResult => numberResult.Value.ToString(CultureInfo.InvariantCulture),
                        _ => b.ToString(),
                    };

                    throw new InvalidIndexOperatorException(null, indexRepresentation, a.ResultType, "access");
            }
        });
    }

    private readonly string debugName;
    private readonly Expression left;
    private readonly Expression right;
    private readonly Func<ExpressionResult, ExpressionResult, ExpressionResult> combine;

    public BinaryExpression(string debugName, Expression left, Expression right, Func<ExpressionResult, ExpressionResult, ExpressionResult> combine)
    {
        this.debugName = debugName;
        this.left = left;
        this.right = right;
        this.combine = combine;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var leftValue = await left.EvaluateAsync(interpreter, cancellationToken);
        var rightValue = await right.EvaluateAsync(interpreter, cancellationToken);

        return combine.Invoke(leftValue, rightValue);
    }

    protected override string DebugDisplay() => $"({left}) {debugName} ({right})";
}