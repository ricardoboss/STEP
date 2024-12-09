namespace StepLang.Framework.Pure;

public class PrintlnFunction : PrintFunction
{
	public new const string Identifier = "println";

	/// <inheritdoc />
	protected override void Print(TextWriter output, string value)
	{
		output.WriteLine(value.AsMemory());
	}
}
