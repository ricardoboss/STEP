using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class Variable
{
    public string Identifier { get; }

    public IReadOnlyList<ResultType> Types { get; }

    public string TypeString => string.Join("|", Types.Except(new[] { ResultType.Null }).Select(t => t.ToTypeName()));

    public bool Nullable { get; }

    public ExpressionResult Value { get; private set; }

    public Variable(string identifier, IReadOnlyList<ResultType> types, bool nullable)
    {
        Identifier = identifier;
        Types = types;
        Nullable = nullable;
        Value = VoidResult.Instance;
    }

    public void Assign(TokenLocation assignmentLocation, ExpressionResult newValue)
    {
        if (!Accepts(newValue))
        {
            if (Nullable || newValue is NullResult)
                throw new IncompatibleVariableTypeException(assignmentLocation, this, newValue);

            throw new NonNullableVariableAssignmentException(assignmentLocation, this, newValue);
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
        return $"{TypeString}{(Nullable ? "?" : "")} {Identifier} = {Value}";
    }
}