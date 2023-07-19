using System.Runtime.CompilerServices;
using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public abstract class Expression
{
    public static Expression FromSymbol(Token symbol, Expression left, Expression right)
    {
        return symbol.Type switch
        {
            TokenType.PlusSymbol => Add(left, right),
            TokenType.MinusSymbol => Subtract(left, right),
            TokenType.AsteriskSymbol => Multiply(left, right),
            TokenType.SlashSymbol => Divide(left, right),
            TokenType.PercentSymbol => Modulo(left, right),
            TokenType.LessThanSymbol => LessThan(left, right),
            TokenType.GreaterThanSymbol => GreaterThan(left, right),
            TokenType.EqualsSymbol => Equals(left, right),
            _ => throw new UnexpectedTokenException(symbol, TokenType.PlusSymbol, TokenType.MinusSymbol,
                TokenType.AsteriskSymbol, TokenType.SlashSymbol, TokenType.PercentSymbol),
        };
    }

    public static Expression Add(Expression left, Expression right)
    {
        return new EvaluatedExpression("+", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new InterpreterException($"Cannot add values of type {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value + b.Value);
        });
    }

    public static Expression Subtract(Expression left, Expression right)
    {
        return new EvaluatedExpression("-", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "double")
                throw new InterpreterException($"Cannot subtract values of type {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value - b.Value);
        });
    }

    public static Expression Multiply(Expression left, Expression right)
    {
        return new EvaluatedExpression("*", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "double")
                throw new InterpreterException($"Cannot multiply values of type {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value * b.Value);
        });
    }

    public static Expression Divide(Expression left, Expression right)
    {
        return new EvaluatedExpression("/", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "double")
                throw new InterpreterException($"Cannot divide values of type {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value / b.Value);
        });
    }

    public static Expression Modulo(Expression left, Expression right)
    {
        return new EvaluatedExpression("%", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType || a.ValueType != "double")
                throw new InterpreterException($"Cannot modulo values of type {a.ValueType} and {b.ValueType}");

            return new(a.ValueType, a.Value % b.Value);
        });
    }

    public static Expression LessThan(Expression left, Expression right)
    {
        return new EvaluatedExpression("<", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new InterpreterException($"Cannot compare values of type {a.ValueType} and {b.ValueType}");

            return new("bool", a.Value < b.Value);
        });
    }

    public static Expression GreaterThan(Expression left, Expression right)
    {
        return new EvaluatedExpression(">", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                throw new InterpreterException($"Cannot compare values of type {a.ValueType} and {b.ValueType}");

            return new("bool", a.Value > b.Value);
        });
    }

    public static Expression Equals(Expression left, Expression right)
    {
        return new EvaluatedExpression("==", left, right, (a, b) =>
        {
            if (a.ValueType != b.ValueType)
                return new("bool", false);

            return new("bool", a.Value == b.Value);
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

    public abstract Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken);

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

        public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken)
        {
            return Task.FromResult(result);
        }

        protected override string DebugDisplay() => result.ToString();
    }

    public class EvaluatedExpression : Expression
    {
        private readonly string debugName;
        private readonly Expression left;
        private readonly Expression right;
        private readonly Func<ExpressionResult, ExpressionResult, ExpressionResult> combine;

        public EvaluatedExpression(string debugName, Expression left, Expression right, Func<ExpressionResult, ExpressionResult, ExpressionResult> combine)
        {
            this.debugName = debugName;
            this.left = left;
            this.right = right;
            this.combine = combine;
        }

        public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken)
        {
            var leftValue = await left.EvaluateAsync(interpreter, cancellationToken);
            var rightValue = await right.EvaluateAsync(interpreter, cancellationToken);

            return combine.Invoke(leftValue, rightValue);
        }

        protected override string DebugDisplay() => $"({left}) {debugName} ({right})";
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

        public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken)
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
                    var line = await interpreter.StdIn.ReadLineAsync(cancellationToken);

                    return new("string", line);

                case "StdOut.Write":
                    var stringArgs = await EvaluateArgs(cancellationToken).Select(r => r.Value?.ToString() ?? string.Empty).Cast<string>().ToListAsync(cancellationToken: cancellationToken);

                    await interpreter.StdOut.WriteAsync(string.Join("", stringArgs));

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

        public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken)
        {
            var variable = interpreter.CurrentScope.GetByIdentifier(Identifier.Value);

            return Task.FromResult<ExpressionResult>(new(variable.TypeName, variable.Value));
        }

        protected override string DebugDisplay() => Identifier.ToString();
    }
}