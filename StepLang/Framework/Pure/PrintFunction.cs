using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class PrintFunction : NativeFunction
{
	public const string Identifier = "print";

	protected override IEnumerable<NativeParameter> NativeParameters =>
	[
		new(Enum.GetValues<ResultType>(), "...values"),
	];

	protected override IEnumerable<ResultType> ReturnTypes =>
	[
		ResultType.Void,
	];

	/// <inheritdoc />
	public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<ExpressionNode> arguments)
	{
		if (interpreter.StdOut is not { } stdOut)
		{
			return VoidResult.Instance;
		}

		var stringArgs = arguments
			.EvaluateUsing(interpreter)
			.Select(Render)
			.ToList();

		Print(stdOut, string.Join("", stringArgs));
		stdOut.Flush();

		return VoidResult.Instance;
	}

	protected virtual string Render(ExpressionResult result)
	{
		return ToStringFunction.Render(result);
	}

	protected virtual void Print(TextWriter output, string value)
	{
		output.Write(value.AsMemory());
	}
}
