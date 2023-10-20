using StepLang.Expressions.Results;

namespace StepLang.Interpreting;

public class Variable
{
    public string Identifier { get; }
    public ResultType Type { get; }
    public bool Nullable { get; }
    public ExpressionResult Value { get; private set; }

    public Variable(string identifier, ResultType type, bool nullable, ExpressionResult value)
    {
        Identifier = identifier;
        Type = type;
        Nullable = nullable;
        Value = value;
    }

    public void Assign(ExpressionResult newValue)
    {
        if (!Accepts(newValue))
        {
            if (Nullable || newValue.ResultType is ResultType.Null)
                throw new IncompatibleVariableTypeException(this, newValue);

            throw new NonNullableVariableAssignmentException(this, newValue);
        }

        Value = newValue;
    }

    public bool Accepts(ExpressionResult value)
    {
        if (value is VoidResult)
            return false; // can never assign void to a variable

        if (value.ResultType is ResultType.Null && Nullable)
            return true;

        return value.ResultType == Type;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Type}{(Nullable ? "?" : "")} {Identifier} = {Value}";
    }
}