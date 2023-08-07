using System.Diagnostics.CodeAnalysis;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework;

public abstract class NativeFunction : FunctionDefinition
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugBodyString => "[native code]";
}