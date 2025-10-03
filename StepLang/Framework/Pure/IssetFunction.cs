using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class IssetFunction : NativeFunction
{
	public const string Identifier = "isset";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(AnyType, "variable"),
	];

	public override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter,
		IReadOnlyList<ExpressionNode> arguments)
	{
		CheckArgumentCount(callLocation, arguments);

		var exp = arguments.Single();
		if (exp is not IdentifierExpressionNode varExp)
			throw new InvalidExpressionTypeException(callLocation, "an identifier", exp.GetType().Name);

		return interpreter.CurrentScope.TryGetVariable(varExp.Identifier.Value, out _);
	}
}
