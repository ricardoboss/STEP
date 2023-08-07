using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
internal sealed class MissingValueExpressionException : ParserException
{
    public MissingValueExpressionException(Token token, Exception inner) : base(token, "A value was expected, but none was found", inner)
    {
    }
}