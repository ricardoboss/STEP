using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class Variable(string identifier, IReadOnlyList<ResultType> types, bool nullable)
	: BaseVariable
{
	public string Identifier { get; } = identifier;

	public IReadOnlyList<ResultType> Types { get; } = types;

	public string TypeString => string.Join("|", Types.Except([ResultType.Null]).Select(t => t.ToTypeName()));

	public bool Nullable { get; } = nullable;

	private ExpressionResult value = VoidResult.Instance;

	public override ExpressionResult Value => value;

	public override void Assign(TokenLocation assignmentLocation, ExpressionResult newValue)
	{
		if (!Accepts(newValue))
		{
			if (Nullable || newValue is NullResult)
			{
				throw new IncompatibleVariableTypeException(assignmentLocation, this, newValue);
			}

			throw new NonNullableVariableAssignmentException(assignmentLocation, this, newValue);
		}

		value = newValue;
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
