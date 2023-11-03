using StepLang.Expressions.Results;

namespace StepLang.Interpreting;

public class NonNullableVariableAssignmentException : InvalidOperationException
{
    public Variable Variable { get; }
    public ExpressionResult NewValue { get; }
    public string HelpText { get; }

    public NonNullableVariableAssignmentException(Variable variable, ExpressionResult newValue) : base($"Cannot assign a value of type {newValue.ResultType.ToTypeName()} to a variable declared as {variable.Value.ResultType.ToTypeName()}")
    {
        Variable = variable;
        NewValue = newValue;
        HelpText = $"Make sure the value you are trying to assign to {variable.Identifier} is of type {variable.Value.ResultType.ToTypeName()} and not {ResultType.Null.ToTypeName()}.";
    }
}
