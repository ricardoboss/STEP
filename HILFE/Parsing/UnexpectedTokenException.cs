using HILFE.Tokenizing;

namespace HILFE.Parsing;

public class UnexpectedTokenException : ParserException
{
    public readonly IReadOnlyCollection<TokenType> Allowed;
    public readonly Token Token;

    public UnexpectedTokenException(Parser.State state, IReadOnlyCollection<TokenType> allowed, Token token) : base(state, $"Got '{token}' but expected one of '{string.Join(", ", allowed)}' (state: {state})")
    {
        Allowed = allowed;
        Token = token;
    }
}