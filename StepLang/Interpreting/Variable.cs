using StepLang.Parsing.Expressions;

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
        newValue.ThrowIfVoid();

        if (newValue.ValueType is not "null" && newValue.ValueType != Value.ValueType)
            throw new InvalidOperationException($"Cannot assign a value of a type {newValue.ValueType} to a variable declared as {Value.ValueType}.");

        Value = newValue;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Identifier} = {Value}";
    }
}