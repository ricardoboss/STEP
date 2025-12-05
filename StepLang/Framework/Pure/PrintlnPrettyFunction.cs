using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;

namespace StepLang.Framework.Pure;

public class PrintlnPrettyFunction : PrintFunction
{
	public new const string Identifier = "printlnPretty";

	protected override string Render(ExpressionResult result)
	{
		return ToStringFunction.Render(result, true);
	}

	/// <inheritdoc />
	protected override void Print(TextWriter output, string value)
	{
		using var span = Telemetry.Profile();

		output.WriteLine(value.AsMemory());
	}
}
