using System.Diagnostics.CodeAnalysis;
using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

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
            _ => throw new NotImplementedException($"The operator {op} is not implemented yet"),
        };
    }

    public static Expression Power(Expression left, Expression right)
    {
        return new BinaryExpression("^", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleTypesException(a.ValueType, b.Value, "pow");

            return new(a.ValueType, Math.Pow(a.Value, b.Value));
        });
    }

    public static Expression Add(Expression left, Expression right)
    {
        return new BinaryExpression("+", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleTypesException(a.ValueType, b.Value, "add");

            return new(a.ValueType, a.Value + b.Value);
        });
    }

    public static Expression Subtract(Expression left, Expression right)
    {
        return new BinaryExpression("-", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleTypesException(a.ValueType, b.Value, "subtract");

            return new(a.ValueType, a.Value - b.Value);
        });
    }

    public static Expression Multiply(Expression left, Expression right)
    {
        return new BinaryExpression("*", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleTypesException(a.ValueType, b.Value, "multiply");

            return new(a.ValueType, a.Value * b.Value);
        });
    }

    public static Expression Divide(Expression left, Expression right)
    {
        return new BinaryExpression("/", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleTypesException(a.ValueType, b.Value, "divide");

            return new(a.ValueType, a.Value / b.Value);
        });
    }

    public static Expression Modulo(Expression left, Expression right)
    {
        return new BinaryExpression("%", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "number")
                throw new IncompatibleTypesException(a.ValueType, b.Value, "modulo");

            return new(a.ValueType, a.Value % b.Value);
        });
    }

    public static Expression LessThan(Expression left, Expression right)
    {
        return new BinaryExpression("<", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleTypesException(a.ValueType, b.Value, "compare");

            return new("bool", a.Value < b.Value);
        });
    }

    public static Expression LessThanOrEqual(Expression left, Expression right)
    {
        return new BinaryExpression("<=", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleTypesException(a.ValueType, b.Value, "compare");

            return new("bool", a.Value <= b.Value);
        });
    }

    public static Expression GreaterThan(Expression left, Expression right)
    {
        return new BinaryExpression(">", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleTypesException(a.ValueType, b.Value, "compare");

            return new("bool", a.Value > b.Value);
        });
    }

    public static Expression GreaterThanOrEqual(Expression left, Expression right)
    {
        return new BinaryExpression(">=", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new IncompatibleTypesException(a.ValueType, b.Value, "compare");

            return new("bool", a.Value >= b.Value);
        });
    }

    public static Expression Equals(Expression left, Expression right)
    {
        return new BinaryExpression("==", left, right, (a, b) =>
        {
            if (a.ValueType == b.ValueType)
                return new("bool", a.Value == b.Value);

            if (a.ValueType == "null") return new("bool", b.Value == null);
            if (b.ValueType == "null") return new("bool", a.Value == null);

            return new("bool", false);
        });
    }

    public static Expression LogicalOr(Expression left, Expression right)
    {
        return new BinaryExpression("||", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "bool")
                throw new IncompatibleTypesException(a.ValueType, b.Value, "compare");

            return new("bool", a.Value || b.Value);
        });
    }
    
    public static Expression LogicalAnd(Expression left, Expression right)
    {
        return new BinaryExpression("&&", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "bool")
                throw new IncompatibleTypesException(a.ValueType, b.Value, "compare");
            
            return new("bool", a.Value && b.Value);
        });
    }
    
    public static Expression Coalesce(Expression left, Expression right)
    {
        return new BinaryExpression("??", left, right, (a, b) =>
        {
            if (a.ValueType == "null")
                return b;

            if (a.ValueType != b.ValueType)
                throw new IncompatibleTypesException(a.ValueType, b.Value, "coalesce");

            return new(a.ValueType, a.Value ?? b.Value);
        });
    }

    public static Expression Not(Expression expression)
    {
        return new UnaryExpression("!", expression, result =>
        {
            var value = result.ExpectBool();

            return new("bool", !value);
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