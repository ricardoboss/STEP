using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
internal sealed class UnexpectedEndOfExpressionException : ParserException
{
    public UnexpectedEndOfExpressionException(Token? token) : base(token, "Unexpected end of expression")
    {
    }
}