using System.Runtime.CompilerServices;
using System.Text;

namespace HILFE.Tokenizing;

public class Tokenizer
{
    private readonly StringBuilder tokenBuilder = new();
    private bool inString;
    private char? stringQuote;
    private bool escaped;

    private Token FinalizeToken(TokenType type, bool clear = true)
    {
        var value = tokenBuilder.ToString();

        if (clear)
            tokenBuilder.Clear();

        return new(type, value);
    }

    public async IAsyncEnumerable<Token> TokenizeAsync(IAsyncEnumerable<char> characters,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var c in characters.WithCancellation(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (inString)
            {
                var token = HandleString(c);
                if (token is not null)
                    yield return token;

                continue;
            }

            if (c is '"' or '\'')
            {
                stringQuote = c;
                inString = true;

                continue;
            }

            var tokens = HandleChar(c);
            if (tokens is null)
                continue;

            foreach (var token in tokens)
                yield return token;
        }
    }

    private Token? HandleString(char c)
    {
        if (c is ' ')
        {
            tokenBuilder.Append(c);

            escaped = false;
        }
        else if (c == stringQuote)
        {
            if (escaped)
            {
                tokenBuilder.Append(c);

                escaped = false;
            }
            else
            {
                inString = false;
                stringQuote = null;

                return FinalizeToken(TokenType.LiteralString);
            }
        }
        else if (c is '\\' && !escaped)
            escaped = true;
        else
            tokenBuilder.Append(c);

        return null;
    }

    private Token []? HandleChar(char c)
    {
        var addWhitespaceToken = false;
        string tokenValue;
        Token? token;
        var whitespaceToken = new Token(TokenType.Whitespace, " ");
        if (c is not ' ')
        {
            if (c.TryParseSymbol(out var tmpType))
                return new []
                {
                    new Token(tmpType.Value, c.ToString()),
                };

            tokenBuilder.Append(c);
        }
        else
        {
            tokenValue = tokenBuilder.ToString();
            if (tokenValue.IsValidIdentifier())
                return new []
                {
                    FinalizeToken(TokenType.Identifier),
                    whitespaceToken,
                };

            addWhitespaceToken = true;
        }

        tokenValue = tokenBuilder.ToString();
        token = HandleTokenValue(tokenValue);
        if (token is null)
        {
            if (addWhitespaceToken)
            {
                return new []
                {
                    whitespaceToken,
                };
            }

            return null;
        }

        if (addWhitespaceToken)
            return new []
            {
                token,
                whitespaceToken,
            };

        return new []
        {
            token,
        };
    }

    private Token? HandleTokenValue(string tokenValue)
    {
        if (tokenValue.IsKnownTypeName())
            return FinalizeToken(TokenType.TypeName);

        if (tokenValue.TryParseKeyword(out var tmpType))
            return FinalizeToken(tmpType.Value);

        if (double.TryParse(tokenValue, out _))
            return FinalizeToken(TokenType.LiteralNumber);

        if (bool.TryParse(tokenValue, out _))
            return FinalizeToken(TokenType.LiteralBoolean);

        return null;
    }

    public void End()
    {
        if (tokenBuilder.Length == 0)
            return;

        if (inString)
            throw new TokenizerException("Unclosed string");

        throw new TokenizerException("Unexpected end of input");
    }

    ~Tokenizer() => End();
}