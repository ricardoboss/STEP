using System.Text;

namespace HILFE;

public static class Tokenizer
{
    public static IEnumerable<Token> Tokenize(string line)
    {
        var tokenBuilder = new StringBuilder();
        var inString = false;
        char? stringQuote = null;
        var escaped = false;
        Token? lastToken = null;

        Token FinalizeToken(TokenType type, bool clear = true)
        {
            var value = tokenBuilder.ToString().Trim();

            if (clear)
                tokenBuilder.Clear();

            return lastToken = new(type, value);
        }

        foreach (var c in line)
        {
            if (inString)
            {
                if (c is ' ')
                {
                    tokenBuilder.Append(c);

                    escaped = false;
                }
                else if (c == stringQuote && !escaped)
                {
                    inString = false;
                    stringQuote = null;

                    yield return FinalizeToken(TokenType.LiteralString);
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

                    continue;
                }

                if (tokenValue.TryParseKeyword(out tmpType))
                {
                    yield return FinalizeToken(tmpType.Value);

                    continue;
                }

                if (tokenValue.Length > 0)
                {
                    yield return FinalizeToken(TokenType.Identifier);

                    continue;
                }

                tokenBuilder.Append(c);

                yield return FinalizeToken(TokenType.Whitespace);
            }
            else if (c.TryParseSymbol(lastToken?.Type, out tmpType))
            {
                yield return FinalizeToken(tmpType.Value);
            }
            else
                tokenBuilder.Append(c);
        }

        var leftoverTokenValue = tokenBuilder.ToString().Trim();
        if (leftoverTokenValue.Length == 0)
            yield return FinalizeToken(TokenType.Whitespace);
        else if (double.TryParse(leftoverTokenValue, out _))
            yield return FinalizeToken(TokenType.LiteralNumber);
        else
            throw new TokenizerException($"Unknown token: {leftoverTokenValue}");
    }
}