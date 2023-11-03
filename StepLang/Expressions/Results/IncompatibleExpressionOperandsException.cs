using StepLang.Interpreting;

namespace StepLang.Expressions.Results;

public class IncompatibleExpressionOperandsException : IncompatibleTypesException
{
    public IncompatibleExpressionOperandsException(ExpressionResult a, string operation) : base(2, null, $"Cannot use the {operation} on values of type {a.ResultType.ToTypeName()}", "Make sure the operand has a type that is compatible with the operator you are trying to use.") { }

    public IncompatibleExpressionOperandsException(ExpressionResult a, ExpressionResult b, string operation) : base(2, null, $"Cannot use the {operation} operator on values of type {a.ResultType.ToTypeName()} and {b.ResultType.ToTypeName()}", "Make sure the operands are of the same type or check if the used operator can be used on the given types.")
    {
    }
}