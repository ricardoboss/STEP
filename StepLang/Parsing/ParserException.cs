using StepLang.Tokenizing;

namespace StepLang.Parsing;

public abstract class ParserException : StepLangException
{
    protected ParserException(Token? token, string message, string helpText, Exception? inner = null) : this(token?.Location, message, helpText, inner)
    {
    }

    protected ParserException(TokenLocation? location, string message, string helpText, Exception? inner = null) : base(location, message, helpText, inner)
    {
    }
}