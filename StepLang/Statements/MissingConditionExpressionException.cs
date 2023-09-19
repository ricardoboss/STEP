using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Statements;

internal sealed class MissingConditionExpressionException : ParserException
{
    public MissingConditionExpressionException(Token token, Exception inner) : base(4, token, "A condition was expected, but none was found", $"Make sure your condition is between the parentheses after the {TokenType.IfKeyword.ToDisplay()} and the following braces are placed correctly.", inner)
    {
    }
}