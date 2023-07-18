using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing;

public abstract class Expression
{
    public static Expression Add(Expression left, Expression right)
    {
        return new EvaluatedExpression(left, right, (a, b) => a + b);
    }

    public static Expression Subtract(Expression left, Expression right)
    {
        return new EvaluatedExpression(left, right, (a, b) => a - b);
    }
    
    public static Expression Multiply(Expression left, Expression right)
    {
        return new EvaluatedExpression(left, right, (a, b) => a * b);
    }
    
    public static Expression Divide(Expression left, Expression right)
    {
        return new EvaluatedExpression(left, right, (a, b) => a / b);
    }

    public static Expression Constant(double value)
    {
        return new ConstantExpression(value);
    }

    public static Expression Constant(string value)
    {
        return new ConstantExpression(value);
    }

    public static Expression Constant(bool value)
    {
        return new ConstantExpression(value);
    }

    public abstract ExpressionResult Evaluate(Interpreter interpreter);

    public class ConstantExpression : Expression
    {
        private readonly dynamic? value;

        public ConstantExpression(dynamic? value)
        {
            this.value = value;
        }

        public override ExpressionResult Evaluate(Interpreter interpreter)
        {
            return new(value);
        }
    }

    public class EvaluatedExpression : Expression
    {
         private readonly Expression left;
         private readonly Expression right;
         private readonly Func<dynamic?, dynamic?, dynamic?> combine;

         public EvaluatedExpression(Expression left, Expression right, Func<dynamic?, dynamic?, dynamic?> combine)
         {
             this.left = left;
             this.right = right;
             this.combine = combine;
         }

         public override ExpressionResult Evaluate(Interpreter interpreter)
        {
            var leftValue = left.Evaluate(interpreter);
            var rightValue = right.Evaluate(interpreter);

            return new(combine.Invoke(leftValue, rightValue));
        }
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

        public override ExpressionResult Evaluate(Interpreter interpreter)
        {
            throw new NotImplementedException("Function calls not implemented yet.");
        }
    }
    
    public class VariableExpression : Expression
    {
        private readonly Token identifier;

        public VariableExpression(Token identifier)
        {
            this.identifier = identifier;
        }

        public override ExpressionResult Evaluate(Interpreter interpreter)
        {
            var variable = interpreter.CurrentScope.GetByIdentifier(identifier.Value);

            return new(variable.Value);
        }
    }
}