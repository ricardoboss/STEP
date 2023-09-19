using StepLang.Expressions.Results;

namespace StepLang.Interpreting;

public class Variable
{
    public string Identifier { get; }
    public ExpressionResult Value { get; private set; }

    public Variable(string identifier, ExpressionResult value)
    {
        Identifier = identifier;
        Value = value;
    }

    public void Assign(ExpressionResult newValue)
    {
        if (newValue is VoidResult || newValue.ResultType != Value.ResultType)
            throw new IncompatibleVariableTypeException(this, newValue);

        Value = newValue;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Identifier} = {Value}";
    }
}