using StepLang.Interpreting;

namespace StepLang.Parsing.Expressions;

public class IncompatibleExpressionOperandsException : IncompatibleTypesException
{
    public IncompatibleExpressionOperandsException(ExpressionResult a, ExpressionResult b, string operation) : base(3, null, $"Cannot use the {operation} operator on values of type {a.ResultType.ToTypeName()} and {b.ResultType.ToTypeName()}", "Make sure the operands are of the same type or check if the used operator can be used on the given types")
    {
    }
}