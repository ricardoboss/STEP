using HILFE.Interpreting;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework;

public abstract class NativeFunction : FunctionDefinition
{
    /// <inheritdoc />
    protected override string DebugBodyString => "[native code]";
}