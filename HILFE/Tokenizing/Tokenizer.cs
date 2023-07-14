using System.Runtime.CompilerServices;
using System.Text;

namespace HILFE.Tokenizing;

public static class Tokenizer
{
    public static async IAsyncEnumerable<Token> TokenizeAsync(IAsyncEnumerable<char> characters, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var tokenBuilder = new StringBuilder();
        var inString = false;
        char? stringQuote = null;
        var escaped = false;

        Token FinalizeToken(TokenType type, bool clear = true)
        {
            var value = tokenBuilder.ToString().Trim();

            if (clear)
                tokenBuilder.Clear();

            return new(type, value);
        }

        await foreach (var c in characters.WithCancellation(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (inString)
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

                        yield return FinalizeToken(TokenType.LiteralString);
                    }
                }
                else if (c is '\\' && !escaped)
                {
                    escaped = true;
                }
                else
                {
                    tokenBuilder.Append(c);
                }

                continue;
            }

            if (c is '"' or '\'')
            {
                stringQuote = c;
                inString = true;

                continue;
            }

            TokenType? tmpType;
            var tokenValue = tokenBuilder.ToString();
            if (c is ' ')
            {
                if (tokenValue.IsKnownTypeName())
                {
                    yield return FinalizeToken(TokenType.TypeName);
                }
                else if (tokenValue.TryParseKeyword(out tmpType))
                {
                    yield return FinalizeToken(tmpType.Value);
                }
                else if (tokenValue.Length > 0)
                {
                    if (tokenValue.Length == 1 && tokenValue[0].TryParseSymbol(out tmpType))
                    {
                        yield return FinalizeToken(tmpType.Value);

                        continue;
                    }

                    yield return FinalizeToken(TokenType.Identifier);
                }

                tokenBuilder.Append(c);

                yield return FinalizeToken(TokenType.Whitespace);
            }
            else if (c.TryParseSymbol(out tmpType))
            {
                if (tokenValue.Length > 0)
                {
                    if (tokenValue.IsValidIdentifier())
                    {
                        yield return FinalizeToken(TokenType.Identifier);
                    }
                    else if (double.TryParse(tokenValue, out _))
                    {
                        yield return FinalizeToken(TokenType.LiteralNumber);
                    }
                }

                tokenBuilder.Append(c);

                yield return FinalizeToken(tmpType.Value);
            }
            else
                tokenBuilder.Append(c);
        }

        var leftoverTokenValue = tokenBuilder.ToString().Trim();
        if (double.TryParse(leftoverTokenValue, out _))
            yield return FinalizeToken(TokenType.LiteralNumber);
        else if (leftoverTokenValue.Length > 0)
            if (leftoverTokenValue.TryParseKeyword(out var tmpType))
                yield return FinalizeToken(tmpType.Value);
            else if (leftoverTokenValue.IsKnownTypeName())
                yield return FinalizeToken(TokenType.TypeName);
            else
                yield return FinalizeToken(TokenType.Identifier);
    }
}