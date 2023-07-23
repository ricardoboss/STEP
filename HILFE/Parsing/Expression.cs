using System.Runtime.CompilerServices;
using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

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

    public class ConstantExpression : Expression
    {
        private readonly ExpressionResult result;

        public ConstantExpression(string type, dynamic? value)
        {
            result = new(type, value);
        }

        public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(result);
        }

        protected override string DebugDisplay() => result.ToString();
    }

    public class BinaryExpression : Expression
    {
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

    public class UnaryExpression : Expression
    {
        private readonly string debugName;
        private readonly Expression expression;
        private readonly Func<ExpressionResult, ExpressionResult> transform;

        public UnaryExpression(string debugName, Expression expression, Func<ExpressionResult, ExpressionResult> transform)
        {
            this.debugName = debugName;
            this.expression = expression;
            this.transform = transform;
        }

        public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
        {
            var result = await expression.EvaluateAsync(interpreter, cancellationToken);

            return transform.Invoke(result);
        }

        protected override string DebugDisplay() => $"({debugName} ({expression})";
    }

    public class FunctionCallExpression : Expression
    {
        private readonly Token identifier;
        private readonly IReadOnlyList<Expression> args;

        public FunctionCallExpression(Token identifier, IReadOnlyList<Expression> args)
        {
            this.identifier = identifier;
            this.args = args;
        }

        public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
        {
            async IAsyncEnumerable<ExpressionResult> EvaluateArgs([EnumeratorCancellation] CancellationToken ct)
            {
                foreach (var arg in args)
                    yield return await arg.EvaluateAsync(interpreter, ct);
            }

            var functionVariable = interpreter.CurrentScope.GetByIdentifier(identifier.Value);
            var functionDefinition = functionVariable.Value as string;
            switch (functionDefinition)
            {
                case "StdIn.ReadLine":
                    if (interpreter.StdIn is not { } stdIn)
                        return new("null");

                    var line = await stdIn.ReadLineAsync(cancellationToken);

                    return new("string", line);

                case "StdOut.Write":
                case "StdOut.WriteLine":
                    if (interpreter.StdOut is not { } stdOut)
                        return new("void", IsVoid: true);

                    var stringArgs = await EvaluateArgs(cancellationToken).Select(r => r.Value?.ToString() ?? string.Empty).Cast<string>().ToListAsync(cancellationToken);

                    if (functionDefinition == "StdOut.WriteLine")
                        await stdOut.WriteLineAsync(string.Join("", stringArgs));
                    else
                        await stdOut.WriteAsync(string.Join("", stringArgs));

                    return new("void", IsVoid: true);

                case "Framework.TypeName":
                    var exp = args.Single();
                    if (exp is not VariableExpression varExp)
                        throw new InterpreterException($"Invalid type, expected {nameof(VariableExpression)}, got {exp.GetType().Name}");

                    var variable = interpreter.CurrentScope.GetByIdentifier(varExp.Identifier.Value);

                    return new("string", variable.TypeName);

                default:
                    throw new InterpreterException($"Undefined function: {functionDefinition}");
            }
        }

        protected override string DebugDisplay() => $"{identifier}({string.Join(',', args)})";
    }

    public class VariableExpression : Expression
    {
        public readonly Token Identifier;

        public VariableExpression(Token identifier)
        {
            Identifier = identifier;
        }

        public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
        {
            var variable = interpreter.CurrentScope.GetByIdentifier(Identifier.Value);

            return Task.FromResult<ExpressionResult>(new(variable.TypeName, variable.Value));
        }

        protected override string DebugDisplay() => Identifier.ToString();
    }
}