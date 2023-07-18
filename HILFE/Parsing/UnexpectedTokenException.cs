using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class UnexpectedTokenException : ParserException
{
    public readonly IReadOnlyCollection<TokenType> Allowed;
    public readonly Token Token;

    public UnexpectedTokenException(Token token, params TokenType[] allowed) : base($"Got '{token}' but expected one of '{string.Join(", ", allowed)}'")
    {
        Allowed = allowed;
        Token = token;
    }
}