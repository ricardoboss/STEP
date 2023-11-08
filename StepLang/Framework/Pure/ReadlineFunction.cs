using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class ReadlineFunction : GenericFunction
{
    public const string Identifier = "readline";

    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

    protected override ExpressionResult Invoke(Interpreter interpreter)
    {
        if (interpreter.StdIn is not { } stdIn)
            return NullResult.Instance;

        var line = stdIn.ReadLine();

        return line is null ? NullResult.Instance : new StringResult(line);
    }
}