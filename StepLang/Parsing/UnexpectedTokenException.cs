using StepLang.Tokenizing;

namespace StepLang.Parsing;

public class UnexpectedTokenException : ParserException
{
    private static string BuildMessage(Token token, IReadOnlyList<TokenType> allowed)
    {
        var expectation = allowed.Count switch
        {
            0 => "nothing",
            1 => $"a {allowed[0].ToDisplay()}",
            _ => $"one of {string.Join(", ", allowed.Select(TokenTypes.ToDisplay))}",
        };

        return $"Expected {expectation}, but got {token.Type.ToDisplay()} instead";
    }

    private static string BuildHelpText()
    {
        return "This error is usually caused by a missing token or a typo. Check the syntax of the statement you are trying to use. Also review your code for syntax errors, such as missing parentheses or other punctuation.";
    }

    public UnexpectedTokenException(Token token, params TokenType [] allowed) : this(token, BuildMessage(token, allowed))
    {
    }

    public UnexpectedTokenException(Token token, string message) : base(1, token, message, BuildHelpText())
    {
    }
}