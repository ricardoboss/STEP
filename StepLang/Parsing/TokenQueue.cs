using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class TokenQueue
{
    private readonly LinkedList<Token> tokenList;

    private Token? lastToken;

    public TokenQueue() => tokenList = new();

    public TokenQueue(IEnumerable<Token> tokens) => tokenList = new(tokens);

    public bool IgnoreWhitespace { get; set; }

    public bool TryDequeue([NotNullWhen(true)] out Token? token)
    {
        token = null;
        if (tokenList.Count == 0)
            return false;

        var skip = 0;
        do
        {
            token = tokenList.Skip(skip).FirstOrDefault();
            skip++;
        } while (IgnoreWhitespace && !(token?.Type.HasMeaning() ?? true));

        if (token is null)
            return false;

        lastToken = token;

        for (; skip > 0; skip--)
            tokenList.RemoveFirst();

        return true;
    }

    public Token Dequeue()
    {
        if (!TryDequeue(out var token))
            throw new UnexpectedEndOfTokensException(lastToken?.Location);

        return token;
    }

    public Token[] Dequeue(int count)
    {
        var tokens = new Token[count];

        for (var i = 0; i < count; i++)
            tokens[i] = Dequeue();

        return tokens;
    }

    public bool TryPeek([NotNullWhen(true)] out Token? token) => TryPeek(0, out token);

    public bool TryPeek(int offset, [NotNullWhen(true)] out Token? token)
    {
        var source = tokenList.AsEnumerable();
        if (IgnoreWhitespace)
            source = source.Where(t => t.Type.HasMeaning());

        token = source.Skip(offset).FirstOrDefault();

        return token is not null;
    }

    public bool TryPeekType([NotNullWhen(true)] out TokenType? type) => TryPeekType(0, out type);

    public bool TryPeekType(int offset, [NotNullWhen(true)] out TokenType? type)
    {
        type = null;
        if (!TryPeek(offset, out var token))
            return false;

        type = token.Type;

        return true;
    }

    public Token Peek(int offset = 0)
    {
        if (!TryPeek(offset, out var token))
            throw new UnexpectedEndOfTokensException(lastToken?.Location);

        return token;
    }

    public Token Dequeue(params TokenType[] allowed)
    {
        Token? token;
        do
        {
            if (TryDequeue(out token))
                continue;

            var typeInfo = allowed.Length switch
            {
                0 => "any token",
                1 => $"a {allowed[0].ToDisplay()}",
                _ => $"any one of {string.Join(',', allowed.Select(TokenTypes.ToDisplay))}",
            };

            throw new UnexpectedEndOfTokensException(lastToken?.Location, $"Expected {typeInfo}");
        } while (!token.Type.HasMeaning() && IgnoreWhitespace);

        if (!allowed.Contains(token.Type))
            throw new UnexpectedTokenException(token, allowed);

        return token;
    }

    public bool IsNotEmpty => tokenList.Count > 0;

    public Token? LastToken => lastToken;
}