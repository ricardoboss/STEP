using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;

namespace StepLang.Framework.Pure;

public class PrintlnPrettyFunction : PrintFunction
{
    public new const string Identifier = "printlnPretty";

    protected override string Render(ExpressionResult result) => ToStringFunction.Render(result, pretty: true);

    /// <inheritdoc />
    protected override void Print(TextWriter output, string value)
        => output.WriteLine(value.AsMemory());
}