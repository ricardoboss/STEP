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

        Token FinalizeToken(TokenType type)
        {
            var value = tokenBuilder.ToString().Trim();
            tokenBuilder.Clear();

            if (!allowedTypes.Contains(type))
                throw new TokenizerException($"Unexpected token type {type} with value '{value}'");

            return new(type, value);
        }

        foreach (var c in line)
        {
            switch (inString)
            {
                case false when c is '=':
                    yield return FinalizeToken(TokenType.AssignmentOperator);

                    allowedTypes = TokenTypes.Values;

                    break;
                case false when c is ' ' && tokenBuilder.ToString().Trim() is { Length: > 0 } value:
                    if (value.IsBuiltInType())
                    {
                        yield return FinalizeToken(TokenType.TypeName);

                        allowedTypes = new[] { TokenType.Identifier };
                    }
                    else
                    {
                        yield return FinalizeToken(TokenType.Identifier);

                        allowedTypes = new[] { TokenType.AssignmentOperator };
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
        else
            throw new TokenizerException($"Unknown token: {leftoverTokenValue}");
    }
}