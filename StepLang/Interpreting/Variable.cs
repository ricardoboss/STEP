using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

/// <summary>
/// Represents a variable in the STEP language.
/// </summary>
public class Variable
{
    /// <summary>
    /// The identifier of the variable.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// The types of the variable.
    /// </summary>
    public IReadOnlyList<ResultType> Types { get; }

    public string TypeString => string.Join("|", Types.Except(new[] { ResultType.Null }).Select(t => t.ToTypeName()));

    public bool Nullable { get; }

    /// <summary>
    /// The current value of the variable.
    /// </summary>
    public ExpressionResult Value { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Variable"/> with the given identifier, type, and value.
    /// </summary>
    /// <param name="identifier">The identifier of the variable.</param>
    /// <param name="types">The types of the variable.</param>
    /// <param name="nullable">Whether the variable is nullable, i.e. can be assigned <see cref="NullResult"/>s.</param>
    public Variable(string identifier, IReadOnlyList<ResultType> types, bool nullable)
    {
        Identifier = identifier;
        Types = types;
        Nullable = nullable;
        Value = VoidResult.Instance;
    }

    /// <summary>
    /// Assigns the given value to this variable.
    /// </summary>
    /// <param name="assignmentLocation">The location of the assignment.</param>
    /// <param name="newValue">The value to assign.</param>
    /// <exception cref="IncompatibleVariableTypeException">Thrown if the given value is not compatible with this variable, i.e. this variable has a different <see cref="ResultType"/> than the <paramref name="newValue"/>.</exception>
    /// <exception cref="NonNullableVariableAssignmentException">Thrown if the new value represents a <see cref="NullResult"/> but this variable is not nullable.</exception>
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

    /// <summary>
    /// Returns whether this variable can accept the given value.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns><see langword="true"/> if this variable can accept the given value; otherwise, <see langword="false"/>.</returns>
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