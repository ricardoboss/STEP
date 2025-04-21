using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

public class ToTypeNameFunction : NativeFunction
{
	public const string Identifier = "toTypeName";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(AnyType, "value"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

	/// <inheritdoc />
	public override StringResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<ExpressionNode> arguments)
	{
		CheckArgumentCount(callLocation, arguments);

		var exp = arguments.Single();
		if (exp is not IdentifierExpressionNode varExp)
			throw new InvalidExpressionTypeException(callLocation, "an identifier", exp.GetType().Name);

		// TODO: check that chain only contains one identifier
		var variable = interpreter.CurrentScope.GetVariable(varExp.IdentifierChain.First());

		return variable.TypeString;
	}
}
