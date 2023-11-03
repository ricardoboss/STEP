using StepLang.Expressions.Results;

namespace StepLang.Interpreting;

public class Variable
{
    public string Identifier { get; }
    public IReadOnlyList<ResultType> Types { get; }
    public bool Nullable { get; }
    public ExpressionResult Value { get; private set; }

    public Variable(string identifier, IReadOnlyList<ResultType> types, bool nullable, ExpressionResult value)
    {
        Identifier = identifier;
        Types = types;
        Nullable = nullable;
        Value = value;
    }

    public void Assign(ExpressionResult newValue)
    {
        if (!Accepts(newValue))
        {
            if (Nullable || newValue is NullResult)
                throw new IncompatibleVariableTypeException(this, newValue);

            throw new NonNullableVariableAssignmentException(this, newValue);
        }

        Value = newValue;
    }

    public bool Accepts(ExpressionResult value)
    {
        return value switch
        {
            VoidResult => false, // can never assign void to a variable
            NullResult when Nullable => true,
            _ => Types.Contains(value.ResultType),
        };
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{string.Join("|", Types.Select(t => t.ToTypeName()))}{(Nullable ? "?" : "")} {Identifier} = {Value}";
    }
}