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
        var allowedTypes = TokenTypes.LineStarters;
        Token? lastToken = null;

        Token FinalizeToken(TokenType type)
        {
            var value = tokenBuilder.ToString().Trim();
            tokenBuilder.Clear();

            if (!allowedTypes.Contains(type))
                throw new TokenizerException($"Unexpected token type {type} with value '{value}'");

            return lastToken = new(type, value);
        }

        foreach (var c in line)
        {
            TokenType? tmpType;
            TokenType[]? tmpAllowedTypes;

            switch (inString)
            {
                case false when c.IsTokenSeparator() && tokenBuilder.ToString() is { Length: > 0 } value:
                    if (value.IsBuiltInType())
                    {
                        yield return FinalizeToken(TokenType.TypeName);

                        allowedTypes = new[] { TokenType.Identifier };
                    }
                    else if (value.TryParseSymbol(lastToken?.Type, out tmpType, out tmpAllowedTypes))
                    {
                        yield return FinalizeToken(tmpType.Value);

                        if (tmpAllowedTypes != null)
                            allowedTypes = tmpAllowedTypes;
                    }
                    else if (value.TryParseKeyword(out tmpType))
                    {
                        yield return FinalizeToken(tmpType.Value);

                        allowedTypes = new[] { TokenType.FunctionCallArgumentListOpener };
                    }
                    else
                    {
                        yield return FinalizeToken(TokenType.Identifier);

                        allowedTypes = new[] { TokenType.AssignmentOperator, TokenType.FunctionCallArgumentListOpener };
                    }

                    break;
                case false when c is '"' or '\'':
                    stringQuote = c;
                    inString = true;

                    break;
                case true when c is ' ':
                    tokenBuilder.Append(c);

                    break;
                case true when c == stringQuote && !escaped:
                    inString = false;
                    stringQuote = null;

                    yield return FinalizeToken(TokenType.LiteralString);

                    allowedTypes = TokenTypes.LineStarters.Concat(new[] { TokenType.StringConcatenationWhitespace }).ToArray();

                    break;
                case true when c is '\\' && !escaped:
                    escaped = true;

                    break;
                default:
                    tokenBuilder.Append(c);

                    escaped = false;
                    break;
            }
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