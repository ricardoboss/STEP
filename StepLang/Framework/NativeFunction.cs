using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework;

public abstract class NativeFunction : FunctionDefinition
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugBodyString => "[native code]";

    protected void CheckArgumentCount(IReadOnlyList<Expression> arguments)
    {
        var expectedCount = Parameters.Count();
        if (arguments.Count != expectedCount)
            throw new InvalidArgumentCountException(expectedCount, arguments.Count);
    }

    protected static void CheckArgumentCount(IReadOnlyList<Expression> arguments, int minCount, int maxCount)
    {
        if (arguments.Count < minCount || arguments.Count > maxCount)
            throw new InvalidArgumentCountException(minCount, arguments.Count, maxCount);
    }
}