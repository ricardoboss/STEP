using System.Diagnostics.CodeAnalysis;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework;

public abstract class NativeFunction : FunctionDefinition
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugBodyString => "[native code]";
}