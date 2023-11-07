using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class ReadlineFunction : NativeFunction
{
    public const string Identifier = "readline";

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments);

        if (interpreter.StdIn is not { } stdIn)
            return NullResult.Instance;

        var line = stdIn.ReadLine();

        return line is null ? NullResult.Instance : new StringResult(line);
    }

    protected override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Str, ResultType.Null };
}