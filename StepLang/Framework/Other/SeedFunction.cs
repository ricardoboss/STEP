using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Other;

public class SeedFunction : NativeFunction
{
    public const string Identifier = "seed";

    /// <inheritdoc />
    public override IEnumerable<(ResultType [] types, string identifier)> Parameters => new (ResultType [] types, string identifier)[] { (new [] { ResultType.Number }, "seed") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 0, 1);

        int seed;
        if (arguments.Count == 1)
            seed = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectInteger().RoundedIntValue, cancellationToken);
        else
            seed = (int)DateTime.Now.Ticks;

        interpreter.SetRandomSeed(seed);

        return VoidResult.Instance;
    }
}