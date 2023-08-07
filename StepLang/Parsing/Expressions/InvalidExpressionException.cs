using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
internal sealed class InvalidExpressionException : ParserException
{
    public InvalidExpressionException(Token? token, string message) : base(token, message)
    {
    }
}