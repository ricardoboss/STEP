using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;

namespace StepLang.Parsing.Expressions;

public abstract class Expression
{
    public static Expression FromOperator(BinaryExpressionOperator op, Expression left, Expression right)
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
            BinaryExpressionOperator.NotEqual => Not(Equals(left, right)),
            BinaryExpressionOperator.LogicalAnd => LogicalAnd(left, right),
            BinaryExpressionOperator.LogicalOr => LogicalOr(left, right),
            BinaryExpressionOperator.Coalesce => Coalesce(left, right),
            BinaryExpressionOperator.Index => Index(left, right),
            _ => throw new NotImplementedException($"The operator {op} is not implemented yet"),
        };
    }

    public static Expression Power(Expression left, Expression right)
    {
        return new BinaryExpression("^", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleExpressionOperandsException(a, b, "power");

            return ExpressionResult.Number(Math.Pow(a.Value, b.Value));
        });
    }

    public static Expression Add(Expression left, Expression right)
    {
        return new BinaryExpression("+", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleExpressionOperandsException(a, b, "add");

            return ExpressionResult.From(a.ValueType, a.Value + b.Value);
        });
    }

    public static Expression Subtract(Expression left, Expression right)
    {
        return new BinaryExpression("-", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleExpressionOperandsException(a, b, "subtract");

            return ExpressionResult.Number(a.Value - b.Value);
        });
    }

    public static Expression Multiply(Expression left, Expression right)
    {
        return new BinaryExpression("*", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleExpressionOperandsException(a, b, "multiply");

            return ExpressionResult.Number(a.Value * b.Value);
        });
    }

    public static Expression Divide(Expression left, Expression right)
    {
        return new BinaryExpression("/", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleExpressionOperandsException(a, b, "divide");

            return ExpressionResult.Number(a.Value / b.Value);
        });
    }

    public static Expression Modulo(Expression left, Expression right)
    {
        return new BinaryExpression("%", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleExpressionOperandsException(a, b, "modulo");

            return ExpressionResult.Number(a.Value % b.Value);
        });
    }

    public static Expression LessThan(Expression left, Expression right)
    {
        return new BinaryExpression("<", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (less than)");

            return ExpressionResult.Bool(a.Value < b.Value);
        });
    }

    public static Expression LessThanOrEqual(Expression left, Expression right)
    {
        return new BinaryExpression("<=", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (less than or equal)");

            return ExpressionResult.Bool(a.Value <= b.Value);
        });
    }

    public static Expression GreaterThan(Expression left, Expression right)
    {
        return new BinaryExpression(">", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (greater than)");

            return ExpressionResult.Bool(a.Value > b.Value);
        });
    }

    public static Expression GreaterThanOrEqual(Expression left, Expression right)
    {
        return new BinaryExpression(">=", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleExpressionOperandsException(a, b, "compare (greater than or equal)");

            return ExpressionResult.Bool(a.Value >= b.Value);
        });
    }

    public static Expression Equals(Expression left, Expression right)
    {
        return new BinaryExpression("==", left, right, (a, b) =>
        {
            if (a.ValueType == b.ValueType)
                return ExpressionResult.Bool(a.Value == b.Value);

            if (a.ValueType == "null") return ExpressionResult.Bool(b.Value == null);
            if (b.ValueType == "null") return ExpressionResult.Bool(a.Value == null);

            return ExpressionResult.False;
        });
    }

    public static Expression LogicalOr(Expression left, Expression right)
    {
        return new BinaryExpression("||", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "bool")
                throw new IncompatibleExpressionOperandsException(a, b, "logical or");

            return ExpressionResult.Bool(a.Value || b.Value);
        });
    }
    
    public static Expression LogicalAnd(Expression left, Expression right)
    {
        return new BinaryExpression("&&", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "bool")
                throw new IncompatibleExpressionOperandsException(a, b, "logical and");

            return ExpressionResult.Bool(a.Value && b.Value);
        });
    }

    public static Expression Coalesce(Expression left, Expression right)
    {
        return new BinaryExpression("??", left, right, (a, b) =>
        {
            if (a.ValueType == "null")
                return b;

            if (a.ValueType != b.ValueType)
                throw new IncompatibleExpressionOperandsException(a, b, "null coalesce");

            return ExpressionResult.From(a.ValueType, a.Value ?? b.Value);
        });
    }

    public static Expression Index(Expression left, Expression right)
    {
        return new BinaryExpression("[]", left, right, (a, b) =>
        {
            switch (a.ValueType)
            {
                case "list":
                {
                    var values = a.ExpectList();
                    var index = b.ExpectIntegerIndex(values.Count);

                    return values[index];
                }
                case "map":
                {
                    var pairs = a.ExpectMap();
                    var key = b.ExpectString();

                    return pairs[key];
                }
                case "string":
                {
                    var value = a.ExpectString();
                    var index = b.ExpectIntegerIndex(value.Length);

                    return ExpressionResult.String(value[index].ToString());
                }
                default:
                    throw new InvalidIndexOperatorException(null, b.Value?.ToString() ?? "<null>", a.ValueType, "access");
            }
        });
    }

    public static Expression Not(Expression expression)
    {
        return new UnaryExpression("!", expression, result =>
        {
            var value = result.ExpectBool();

            return ExpressionResult.Bool(!value);
        });
    }

    public static Expression Constant(double value) => ConstantExpression.Number(value);

    public static Expression Constant(string value) => ConstantExpression.String(value);

    public static Expression Constant(bool value) => ConstantExpression.Bool(value);

    public abstract Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var debugDisplay = DebugDisplay();
        if (debugDisplay.Length > 0)
            debugDisplay = $": {debugDisplay}";

        return $"<{GetType().Name}{debugDisplay}>";
    }

    [ExcludeFromCodeCoverage]
    protected virtual string DebugDisplay() => "";
}