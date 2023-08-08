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

    public UnexpectedTokenException(Token token, params TokenType [] allowed) : base(token, BuildMessage(token, allowed))
    {
    }
}