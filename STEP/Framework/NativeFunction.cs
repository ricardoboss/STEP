using System.Diagnostics.CodeAnalysis;
using STEP.Parsing.Expressions;

namespace STEP.Framework;

public abstract class NativeFunction : FunctionDefinition
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugBodyString => "[native code]";
}