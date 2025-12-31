using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;
using StepLang.Tooling.UI;

namespace StepLang.Framework.Ui;

public class DrawTextFunction : GenericFunction<NumberResult, NumberResult, StringResult>
{
	public const string Identifier = "drawText";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyNumber, "x"),
		new(OnlyNumber, "y"),
		new(OnlyString, "text"),
	];

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		NumberResult argument1, NumberResult argument2, StringResult argument3)
	{
		var window = RenderingManager.GetOrCreateWindow(new()
		{
			Width = 840,
			Height = 620,
			Title = "STEP",
		}).Result;

		window.Renderer.DrawText(argument1, argument2, argument3.Value);

		return VoidResult.Instance;
	}
}
