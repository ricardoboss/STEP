using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class Variable
{
    public string Identifier { get; }
    public IReadOnlyList<ResultType> Types { get; }
    public string TypeString => string.Join("|", Types.Select(t => t.ToTypeName()));
    public bool Nullable { get; }
    public ExpressionResult Value { get; private set; }

    public Variable(string identifier, IReadOnlyList<ResultType> types, bool nullable)
    {
        Identifier = identifier;
        Types = types;
        Nullable = nullable;
        Value = VoidResult.Instance;
    }

    public void Assign(TokenLocation location, ExpressionResult newValue)
    {
        if (!Accepts(newValue))
        {
            if (Nullable || newValue is NullResult)
                throw new IncompatibleVariableTypeException(location, this, newValue);

            throw new NonNullableVariableAssignmentException(location, this, newValue);
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