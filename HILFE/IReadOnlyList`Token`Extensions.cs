using HILFE.Tokenizing;

namespace HILFE;

public static class ReadOnlyListTokenExtensions
{
    public static Token Next(this IEnumerable<Token> tokens, int offset, params TokenType[] except)
    {
        return tokens.Without(except).Skip(offset).First();
    }

    public static IEnumerable<Token> Without(this IEnumerable<Token> tokens, params TokenType[] except)
    {
        return tokens.Where(t => !except.Contains(t.Type));
    }

    public static IEnumerable<Token> OnlyMeaningful(this IEnumerable<Token> tokens)
    {
        return tokens.Without(TokenType.Whitespace, TokenType.NewLine);
    }

    public static Token NextMeaningful(this IEnumerable<Token> tokens, int offset)
    {
        return tokens.Next(offset, TokenType.Whitespace, TokenType.NewLine);
    }
}