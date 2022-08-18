namespace HILFE;

public class UnexpectedTokenException : ParserException
{
    public UnexpectedTokenException(IEnumerable<TokenType> allowed, Token token) : base($"Got '{token.Type}' but expected one of '{string.Join(", ", allowed)}' (value: '{token.Value}')")
    {
    }
}