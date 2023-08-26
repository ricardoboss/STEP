using StepLang.Parsing.Expressions;

namespace StepLang.Interpreting;

public class IncompatibleVariableTypeException : IncompatibleTypesException
{
    public IncompatibleVariableTypeException(Variable variable, ExpressionResult newValue) : base(2, null, $"Cannot assign a value of a type {newValue.ResultType.ToTypeName()} to a variable declared as {variable.Value.ResultType.ToTypeName()}", $"Make sure the value you are trying to assign to {variable.Identifier} is of type {variable.Value.ResultType.ToTypeName()}.")
    {
    }
}