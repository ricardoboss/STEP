using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class IncompatibleVariableTypeException : InterpreterException
{
    public IncompatibleVariableTypeException(TokenLocation location, Variable variable, ExpressionResult newValue) : base(6, location, $"Cannot assign a value of type {newValue.ResultType.ToTypeName()} to a variable declared as {variable.TypeString}", $"Make sure the value you are trying to assign to {variable.Identifier} is of type {variable.TypeString}.")
    {
    }
}