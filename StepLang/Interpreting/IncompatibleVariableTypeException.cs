using StepLang.Parsing.Expressions;

namespace StepLang.Interpreting;

public class IncompatibleVariableTypeException : IncompatibleTypesException
{
    public IncompatibleVariableTypeException(Variable variable, ExpressionResult newValue) : base(null, $"Cannot assign a value of a type {newValue.ValueType} to a variable declared as {variable.Value.ValueType}", $"Make sure the value you are trying to assign to {variable.Identifier} is of type {variable.Value.ValueType}.")
    {
    }
}