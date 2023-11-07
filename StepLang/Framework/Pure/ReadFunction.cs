using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class ReadFunction : NativeFunction
{
    public const string Identifier = "read";

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments);

        if (interpreter.StdIn is not { } stdIn)
            return NullResult.Instance;

        var character = stdIn.Read();
        if (character < 0)
            return NullResult.Instance;

        return new StringResult(char.ConvertFromUtf32(character));
    }

    protected override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Str, ResultType.Null };
}