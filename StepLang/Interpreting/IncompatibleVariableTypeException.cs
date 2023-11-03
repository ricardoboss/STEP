using StepLang.Expressions.Results;

namespace StepLang.Interpreting;

public class IncompatibleVariableTypeException : InvalidOperationException
{
    public string HelpText { get; }

    public IncompatibleVariableTypeException(Variable variable, ExpressionResult newValue) : base($"Cannot assign a value of type {newValue.ResultType.ToTypeName()} to a variable declared as {variable.Value.ResultType.ToTypeName()}")
    {
        HelpText = $"Make sure the value you are trying to assign to {variable.Identifier} is of type {variable.Value.ResultType.ToTypeName()}.";
    }
}