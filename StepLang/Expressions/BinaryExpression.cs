using System.Globalization;
using StepLang.Expressions.Results;
using StepLang.Framework;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions;

public abstract class BinaryExpression : Expression
{
    public static BinaryExpression FromOperator(BinaryExpressionOperator op, Expression left, Expression right)
    {
        return op switch
        {
            BinaryExpressionOperator.Power => new PowerExpression(left, right),
            BinaryExpressionOperator.Add => new AddExpression(left, right),
            BinaryExpressionOperator.Subtract => new SubtractExpression(left, right),
            BinaryExpressionOperator.Multiply => new MultiplyExpression(left, right),
            BinaryExpressionOperator.Divide => new DivideExpression(left, right),
            BinaryExpressionOperator.Modulo => new ModuloExpression(left, right),
            BinaryExpressionOperator.GreaterThan => new GreaterThanExpression(left, right),
            BinaryExpressionOperator.GreaterThanOrEqual => new GreaterThanOrEqualExpression(left, right),
            BinaryExpressionOperator.LessThan => new LessThanExpression(left, right),
            BinaryExpressionOperator.LessThanOrEqual => new LessThanOrEqualExpression(left, right),
            BinaryExpressionOperator.Equal => new EqualsExpression(left, right),
            BinaryExpressionOperator.NotEqual => new NotEqualsExpression(left, right),
            BinaryExpressionOperator.LogicalAnd => new LogicalAndExpression(left, right),
            BinaryExpressionOperator.LogicalOr => new LogicalOrExpression(left, right),
            BinaryExpressionOperator.Coalesce => new CoalesceExpression(left, right),
            BinaryExpressionOperator.Index => new IndexExpression(left, right),
            _ => throw new NotImplementedException($"The operator {op} is not implemented yet"),
        };
    }

    private readonly Expression leftExpression;
    private readonly Expression rightExpression;
    private readonly BinaryExpressionOperator @operator;

    private BinaryExpression(Expression leftExpression, Expression rightExpression, BinaryExpressionOperator @operator)
    {
        this.leftExpression = leftExpression;
        this.rightExpression = rightExpression;
        this.@operator = @operator;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var leftValue = await leftExpression.EvaluateAsync(interpreter, cancellationToken);
        var rightValue = await rightExpression.EvaluateAsync(interpreter, cancellationToken);

        return Combine(leftValue, rightValue);
    }

    protected override string DebugDisplay() => $"({leftExpression}) {@operator.ToSymbol()} ({rightExpression})";

    protected abstract ExpressionResult Combine(ExpressionResult left, ExpressionResult right);

    private class AddExpression : BinaryExpression
    {
        public AddExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Add)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            return left switch
            {
                NumberResult aNumber when right is NumberResult bNumber => new NumberResult(aNumber.Value + bNumber.Value),
                NumberResult aNumber when right is StringResult bString => new StringResult(aNumber.Value + bString.Value),
                StringResult aString when right is NumberResult bNumber => new StringResult(aString.Value + bNumber.Value),
                StringResult aString when right is StringResult bString => new StringResult(aString.Value + bString.Value),
                _ => throw new IncompatibleExpressionOperandsException(left, right, "add"),
            };
        }
    }

    private class SubtractExpression : BinaryExpression
    {
        public SubtractExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Subtract)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult a, ExpressionResult b)
        {
            if (a is not NumberResult aNumber || b is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(a, b, "subtract");

            return new NumberResult(aNumber.Value - bNumber.Value);
        }
    }

    private class PowerExpression : BinaryExpression
    {
        public PowerExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Power)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not NumberResult aNumber || right is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(left, right, "power");

            return new NumberResult(Math.Pow(aNumber.Value, bNumber.Value));
        }
    }

    private class MultiplyExpression : BinaryExpression
    {
        public MultiplyExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Multiply)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not NumberResult aNumber || right is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(left, right, "multiply");

            return new NumberResult(aNumber.Value * bNumber.Value);
        }
    }

    private class DivideExpression : BinaryExpression
    {
        public DivideExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Divide)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not NumberResult aNumber || right is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(left, right, "divide");

            // TODO: throw a specific exception when dividing by zero

            return new NumberResult(aNumber.Value / bNumber.Value);
        }
    }

    private class ModuloExpression : BinaryExpression
    {
        public ModuloExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Modulo)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not NumberResult aNumber || right is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(left, right, "modulo");

            return new NumberResult(aNumber.Value % bNumber.Value);
        }
    }

    private class GreaterThanExpression : BinaryExpression
    {
        public GreaterThanExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.GreaterThan)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not NumberResult aNumber || right is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(left, right, "compare (greater than)");

            return new BoolResult(aNumber.Value > bNumber.Value);
        }
    }

    private class GreaterThanOrEqualExpression : BinaryExpression
    {
        public GreaterThanOrEqualExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.GreaterThanOrEqual)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not NumberResult aNumber || right is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(left, right, "compare (greater than or equal)");

            return new BoolResult(aNumber.Value >= bNumber.Value);
        }
    }

    private class LessThanExpression : BinaryExpression
    {
        public LessThanExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.LessThan)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not NumberResult aNumber || right is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(left, right, "compare (less than)");

            return new BoolResult(aNumber.Value < bNumber.Value);
        }
    }

    private class LessThanOrEqualExpression : BinaryExpression
    {
        public LessThanOrEqualExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.LessThanOrEqual)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not NumberResult aNumber || right is not NumberResult bNumber)
                throw new IncompatibleExpressionOperandsException(left, right, "compare (less than or equal)");

            return new BoolResult(aNumber.Value <= bNumber.Value);
        }
    }

    private class EqualsExpression : BinaryExpression
    {
        public EqualsExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Equal)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
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
    }

    private class NotEqualsExpression : BinaryExpression
    {
        public NotEqualsExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.NotEqual)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
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
    }

    private class LogicalAndExpression : BinaryExpression
    {
        public LogicalAndExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.LogicalAnd)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not BoolResult aBool || right is not BoolResult bBool)
                throw new IncompatibleExpressionOperandsException(left, right, "logical and");

            return new BoolResult(aBool.Value && bBool.Value);
        }
    }

    private class LogicalOrExpression : BinaryExpression
    {
        public LogicalOrExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.LogicalOr)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            if (left is not BoolResult aBool || right is not BoolResult bBool)
                throw new IncompatibleExpressionOperandsException(left, right, "logical or");

            return new BoolResult(aBool.Value || bBool.Value);
        }
    }

    private class CoalesceExpression : BinaryExpression
    {
        public CoalesceExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Coalesce)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            return left is NullResult ? right : left;
        }
    }

    private class IndexExpression : BinaryExpression
    {
        public IndexExpression(Expression leftExpression, Expression rightExpression) : base(leftExpression, rightExpression, BinaryExpressionOperator.Index)
        {
        }

        protected override ExpressionResult Combine(ExpressionResult left, ExpressionResult right)
        {
            switch (left.ResultType)
            {
                case ResultType.List:
                {
                    var values = left.ExpectList().Value;
                    var index = right.ExpectInteger().RoundedIntValue;

                    if (index < 0 || index >= values.Count)
                        throw new IndexOutOfBoundsException(index, values.Count);

                    return values[index];
                }
                case ResultType.Map:
                {
                    var pairs = left.ExpectMap().Value;
                    var key = right.ExpectString().Value;

                    return pairs[key];
                }
                case ResultType.Str:
                {
                    var value = left.ExpectString().Value;
                    var index = right.ExpectInteger().RoundedIntValue;
                    var grapheme = value.GraphemeAt(index);
                    if (grapheme == null)
                        throw new IndexOutOfBoundsException(index, value.GraphemeLength());

                    return new StringResult(grapheme);
                }
                default:
                    var indexRepresentation = right switch
                    {
                        StringResult stringResult => stringResult.Value,
                        NumberResult numberResult => numberResult.Value.ToString(CultureInfo.InvariantCulture),
                        _ => right.ToString(),
                    };

                    throw new InvalidIndexOperatorException(null, indexRepresentation, left.ResultType, "access");
            }
        }
    }
}