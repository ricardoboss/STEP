using HILFE.Parsing.Expressions;

namespace HILFE.Interpreting;

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
            throw new IncompatibleTypesException(Value.ValueType, newValue.ValueType, "assign");

        Value = newValue;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Identifier} = {Value}";
    }
}