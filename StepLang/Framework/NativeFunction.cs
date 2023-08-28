using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework;

public abstract class NativeFunction : FunctionDefinition
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugBodyString => "[native code]";

    protected static void CheckArgumentCount(IReadOnlyList<Expression> arguments, int expectedCount)
    {
        if (arguments.Count != expectedCount)
            throw new InvalidArgumentCountException(expectedCount, arguments.Count);
    }
}