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

        if (tokenBuilder.Length == 0)
            yield break;

        var leftOverToken = HandleTokenValue(tokenBuilder.ToString());
        if (leftOverToken is not null) yield return leftOverToken;
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
        string tokenValue;

        if (c.TryParseSymbol(out var symbolType))
        {
            var tokens = new List<Token>();
        
            tokenValue = tokenBuilder.ToString();
            if (tokenValue.Length > 0)
            {
                var tempToken = HandleTokenValue(tokenValue);
                if (tempToken is not null)
                {
                    tokens.Add(tempToken);
                }
                else
                {
                    if (tokenValue.IsValidIdentifier())
                        tokens.Add(FinalizeToken(TokenType.Identifier));
                    else
                        throw new TokenizerException($"Invalid identifier: {tokenValue}");
                }
            }

            tokens.Add(new(symbolType.Value, c.ToString()));
        
            return tokens.ToArray();
        }

        if (IsPartOfLiteralNumber(c))
        {
            tokenBuilder.Append(c);
            return null;
        }

        tokenValue = tokenBuilder.Append(c).ToString();
        var token = HandleTokenValue(tokenValue);
        if (token is null)
            return null;

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
    
    private static bool IsPartOfLiteralNumber(char c)
    {
        return char.IsDigit(c) || c == '.' || c == '-' || c == '+';
    }

    ~Tokenizer() => End();
}