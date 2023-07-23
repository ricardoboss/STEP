using System.Runtime.CompilerServices;
using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Expressions;

public abstract class Expression
{
    public static Expression FromOperator(BinaryExpressionOperator op, Expression left, Expression right)
    {
        return op switch
        {
            BinaryExpressionOperator.Plus => Add(left, right),
            BinaryExpressionOperator.Minus => Subtract(left, right),
            BinaryExpressionOperator.Multiply => Multiply(left, right),
            BinaryExpressionOperator.Divide => Divide(left, right),
            BinaryExpressionOperator.Modulo => Modulo(left, right),
            BinaryExpressionOperator.GreaterThan => GreaterThan(left, right),
            BinaryExpressionOperator.LessThan => LessThan(left, right),
            BinaryExpressionOperator.Equal => Equals(left, right),
            BinaryExpressionOperator.NotEqual => Not(Equals(left, right)),
            BinaryExpressionOperator.LogicalAnd => LogicalAnd(left, right),
            BinaryExpressionOperator.LogicalOr => LogicalOr(left, right),
            BinaryExpressionOperator.Coalesce => Coalesce(left, right),
            _ => throw new NotImplementedException($"The operator {op} is not implemented yet"),
        };
    }

    public static Expression Add(Expression left, Expression right)
    {
        return new BinaryExpression("+", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new InterpreterException($"Cannot add values of types {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value + b.Value);
        });
    }

    public static Expression Subtract(Expression left, Expression right)
    {
        return new BinaryExpression("-", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "double")
                throw new InterpreterException($"Cannot subtract values of types {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value - b.Value);
        });
    }

    public static Expression Multiply(Expression left, Expression right)
    {
        return new BinaryExpression("*", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "double")
                throw new InterpreterException($"Cannot multiply values of types {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value * b.Value);
        });
    }

    public static Expression Divide(Expression left, Expression right)
    {
        return new BinaryExpression("/", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "double")
                throw new InterpreterException($"Cannot divide values of types {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value / b.Value);
        });
    }

    public static Expression Modulo(Expression left, Expression right)
    {
        return new BinaryExpression("%", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "double")
                throw new InterpreterException($"Cannot modulo values of types {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value % b.Value);
        });
    }

    public static Expression LessThan(Expression left, Expression right)
    {
        return new BinaryExpression("<", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new InterpreterException($"Cannot compare values of types {a.ValueType} and {b.ValueType}");

            return new("bool", a.Value < b.Value);
        });
    }

    public static Expression GreaterThan(Expression left, Expression right)
    {
        return new BinaryExpression(">", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new InterpreterException($"Cannot compare values of types {a.ValueType} and {b.ValueType}");

            return new("bool", a.Value > b.Value);
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
                throw new InterpreterException($"Cannot apply '||' to values of types {a.ValueType} and {b.ValueType}");

            return new("bool", a.Value || b.Value);
        });
    }
    
    public static Expression LogicalAnd(Expression left, Expression right)
    {
        return new BinaryExpression("&&", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "bool")
                throw new InterpreterException($"Cannot apply '&&' to values of types {a.ValueType} and {b.ValueType}");
            
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
                throw new InterpreterException($"Cannot apply '??' to values of types {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value ?? b.Value);
        });
    }

    public static Expression Not(Expression expression)
    {
        return new UnaryExpression("!", expression, result =>
        {
            if (result.ValueType != "bool")
                throw new InterpreterException($"Cannot apply 'not' to values of type {result.ValueType}");

            return new("bool", !result.Value);
        });
    }

    public static Expression Constant(double value)
    {
        return new ConstantExpression("double", value);
    }

    public static Expression Constant(string value)
    {
        return new ConstantExpression("string", value);
    }

    public static Expression Constant(bool value)
    {
        return new ConstantExpression("bool", value);
    }

    public abstract Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public override string ToString()
    {
        var debugDisplay = DebugDisplay();
        if (debugDisplay.Length > 0)
            debugDisplay = $": {debugDisplay}";

        return $"<{GetType().Name}{debugDisplay}>";
    }

    protected virtual string DebugDisplay() => "";
}