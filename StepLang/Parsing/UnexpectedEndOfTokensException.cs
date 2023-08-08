using StepLang.Tokenizing;

namespace StepLang.Parsing;

public class UnexpectedEndOfTokensException : ParserException
{
    private const string GeneralHelpText = "The current statement was not complete. Check the syntax of the statement you are trying to use. Also review your code for syntax errors, such as missing parentheses or other punctuation.";

    public UnexpectedEndOfTokensException(TokenLocation? location) : base(location, "Expected a statement", GeneralHelpText)
    {
    }

    public UnexpectedEndOfTokensException(TokenLocation? location, string message) : base(location, message, GeneralHelpText)
    {
    }
}