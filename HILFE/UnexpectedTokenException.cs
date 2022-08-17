namespace HILFE;

public class UnexpectedTokenException : TokenizerException
{
    public UnexpectedTokenException(IEnumerable<TokenType> allowed, TokenType type, string value) : base($"Got '{type}' but expected one of '{string.Join(", ", allowed)}' (value: '{value}')")
    {
    }
}