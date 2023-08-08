using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

internal sealed class MissingConditionExpressionException : ParserException
{
    public MissingConditionExpressionException(Token token, Exception inner) : base(token, "A condition was expected, but none was found", $"Make sure your condition is between the parentheses after the {TokenType.IfKeyword.ToDisplay()} and the following braces are placed correctly.", inner)
    {
    }
}