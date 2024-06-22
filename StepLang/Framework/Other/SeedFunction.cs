using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

/// <summary>
/// Sets the random seed. Calling it with <see cref="NullResult"/> will set the seed to the current time
/// (<see cref="DateTime"/>.<see cref="DateTime.Ticks"/>).
/// </summary>
/// <seealso cref="RandomFunction"/>
public class SeedFunction : GenericFunction<ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="SeedFunction"/>.
    /// </summary>
    public const string Identifier = "seed";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(NullableNumber, "seed"),
    };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ExpressionResult argument1)
    {
        int seed;
        if (argument1 is NumberResult numberResult)
            seed = numberResult;
        else
            seed = (int)DateTime.Now.Ticks;

        interpreter.SetRandomSeed(seed);

        return VoidResult.Instance;
    }
}