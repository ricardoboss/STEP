using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
internal class MissingConditionExpressionException : ParserException
{
    public MissingConditionExpressionException(Token token, Exception inner) : base(token, "A condition was expected, but none was found", inner)
    {
    }
}